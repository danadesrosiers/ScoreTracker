using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
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

                        var existingMeet = await _meetService.GetAsync(meetSearchResult.Id!);
                        if (existingMeet != null && !existingMeet.IsLive() && await HasResults(existingMeet, stoppingToken))
                        {
                            continue;
                        }

                        var meetInfo = await _meetResultsProvider.GetMeetInfoAsync(meetSearchResult.Id!);
                        if (existingMeet == null)
                        {
                            await _meetService.AddAsync(meetInfo.Meet);
                            await Task.WhenAll(meetInfo.Clubs.Select(AddUpdateClubAsync));
                            await Task.WhenAll(meetInfo.Athletes.Select(AddUpdateAthleteAsync));
                        }

                        if (meetInfo.Results != null)
                        {
                            // Only update results that have changed.
                            var lastUpdatedResult = await _meetResultService.GetAsync(new ResultsQuery
                            {
                                MeetId = meetInfo.Meet.Id,
                                OrderBy = MeetResultOrderBy.LastModified,
                                Limit = 1,
                            }).FirstOrDefaultAsync(stoppingToken);
                            var resultsToUpdate = lastUpdatedResult != null
                                ? meetInfo.Results.Where(result => result.LastUpdated >= lastUpdatedResult.LastUpdated)
                                : meetInfo.Results;
                            await Task.WhenAll(resultsToUpdate.Select(result => _meetResultService.AddAsync(result)));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<bool> HasResults(Meet existingMeet, CancellationToken stoppingToken)
        {
            var resultQuery = new ResultsQuery
            {
                MeetId = existingMeet.Id,
                Limit = 1
            };
            var results = await _meetResultService.GetAsync(resultQuery).ToListAsync(stoppingToken);

            return results.Any();
        }

        private async Task AddUpdateClubAsync(Club club)
        {
            var existingClub = await _clubService.GetAsync(club.Id!);
            if (existingClub == null)
            {
                await _clubService.AddAsync(club);
            }
            else if (club != existingClub)
            {
                try
                {
                    await _clubService.UpdateAsync(club with { ETag = existingClub.ETag });
                }
                catch (CosmosException cre) when (cre.StatusCode == HttpStatusCode.PreconditionFailed)
                {
                    Console.WriteLine($"Retrying Update Athlete {club.Name} due to PreconditionFailed.");
                    await AddUpdateClubAsync(club);
                }
            }
        }

        private async Task AddUpdateAthleteAsync(Athlete athlete)
        {
            var existingAthlete = await _athleteService.GetAsync(athlete.Id!);
            if (existingAthlete == null)
            {
                await _athleteService.AddAsync(athlete);
            }
        }
    }
}