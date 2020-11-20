using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ScoreTracker.Server.Services.Results;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server
{
    public class MeetLoaderService : BackgroundService
    {
        private readonly IMeetService _meetService;
        private readonly IClubService _clubService;
        private readonly IAthleteService _athleteService;
        private readonly IResultService _meetResultService;
        private readonly IMeetResultsProvider _meetResultsProvider;

        public MeetLoaderService(
            IMeetService meetService,
            IClubService clubService,
            IAthleteService athleteService,
            IResultService meetResultService,
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
                             !await HasResults(existingMeet, stoppingToken);
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

        private async Task<bool> HasResults(Meet existingMeet, CancellationToken stoppingToken)
        {
            var resultQuery = new ResultsQuery
            {
                MeetId = existingMeet.Id,
                Limit = 1
            };
            var results = await _meetResultService.GetResultsAsync(resultQuery).ToListAsync(stoppingToken);

            return results.Any();
        }
    }
}