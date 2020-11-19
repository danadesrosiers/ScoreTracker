using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ScoreTracker.Server.Services;
using ScoreTracker.Server.Services.Athletes;
using ScoreTracker.Server.Services.Clubs;
using ScoreTracker.Server.Services.Meets;
using ScoreTracker.Server.Services.Results;

namespace ScoreTracker.Server
{
    public class MeetLoaderService : BackgroundService
    {
        private readonly MeetService _meetService;
        private readonly ClubService _clubService;
        private readonly AthleteService _athleteService;
        private readonly MeetResultService _meetResultService;
        private readonly IMeetResultsProvider _meetResultsProvider;

        public MeetLoaderService(
            MeetService meetService,
            ClubService clubService,
            AthleteService athleteService,
            MeetResultService meetResultService,
            IMeetResultsProvider meetResultsProvider)
        {
            _meetService = meetService;
            _clubService = clubService;
            _athleteService = athleteService;
            _meetResultService = meetResultService;
            _meetResultsProvider = meetResultsProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Add Meets.
                var meets = await _meetResultsProvider.SearchMeetsAsync(new MeetQuery
                {
                    Discipline = Discipline.Men,
                    StateCode = StateCode.Ca,
                    StartDate = DateTime.UtcNow - TimeSpan.FromDays(365)
                });

                foreach (var meetSearchResult in meets)
                {
                    try
                    {
                        if (stoppingToken.IsCancellationRequested) break;

                        var existingMeet = await _meetService.GetMeetAsync(meetSearchResult.Id);
                        var getResults = existingMeet == null ||
                             existingMeet.IsLive() ||
                             !(await _meetResultService.GetResultsAsync(new ResultsQuery(existingMeet.Id), 1)).Any();
                        var meetInfo = await _meetResultsProvider.GetMeetInfoAsync(meetSearchResult.Id, getResults);
                        if (existingMeet == null)
                        {
                            await _meetService.AddMeetAsync(meetInfo.Meet);
                            await Task.WhenAll(meetInfo.Clubs.Select(club => _clubService.AddClubAsync(club)));
                            await Task.WhenAll(meetInfo.Athletes.Select(athlete => _athleteService.AddAthleteAsync(athlete)));
                        }

                        if (meetInfo.Results != null)
                        {
                            await Task.WhenAll(meetInfo.Results.Select(result => _meetResultService.AddResultAsync(result)));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }

            // TODO: Poll all live meets for updates.  Send notifications to subscribers.  (Maybe a separate job)
        }
    }
}