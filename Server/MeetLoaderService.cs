using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using ScoreTracker.Server.MeetResultsProviders;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server
{
    public class MeetLoaderService : BackgroundService
    {
        private readonly IMeetClient _meetClient;
        private readonly IClubClient _clubClient;
        private readonly IAthleteClient _athleteClient;
        private readonly IMeetResultClient _meetResultClient;
        private readonly IMeetResultsProvider _meetResultsProvider;

        public MeetLoaderService(
            IMeetClient meetClient,
            IClubClient clubClient,
            IAthleteClient athleteClient,
            IMeetResultClient meetResultClient,
            IMeetResultsProvider meetResultsProvider)
        {
            _meetClient = meetClient;
            _clubClient = clubClient;
            _athleteClient = athleteClient;
            _meetResultClient = meetResultClient;
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

                        var existingMeet = await _meetClient.GetAsync(new Id(meetSearchResult.Id!));
                        if (existingMeet != null && !existingMeet.IsLive() && await HasResults(existingMeet, stoppingToken))
                        {
                            continue;
                        }

                        var meetInfo = await _meetResultsProvider.GetMeetInfoAsync(meetSearchResult.Id!);
                        if (existingMeet == null)
                        {
                            await _meetClient.AddAsync(meetInfo.Meet);
                            await Task.WhenAll(meetInfo.Clubs.Select(AddUpdateClubAsync));
                            await Task.WhenAll(meetInfo.Athletes.Select(AddUpdateAthleteAsync));
                        }

                        if (meetInfo.Results != null)
                        {
                            // Only update results that have changed.
                            var lastUpdatedResult = await _meetResultClient.GetAsync(new ResultsQuery
                            {
                                MeetId = meetInfo.Meet.Id,
                                OrderBy = MeetResultOrderBy.LastModified,
                                Limit = 1,
                            }).FirstOrDefaultAsync(stoppingToken);
                            var resultsToUpdate = lastUpdatedResult != null
                                ? meetInfo.Results.Where(result => result.LastUpdated >= lastUpdatedResult.LastUpdated)
                                : meetInfo.Results;
                            await Task.WhenAll(resultsToUpdate.Select(result => _meetResultClient.AddAsync(result)));
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
            var results = await _meetResultClient.GetAsync(resultQuery).ToListAsync(stoppingToken);

            return results.Any();
        }

        private async Task AddUpdateClubAsync(Club club)
        {
            var existingClub = await _clubClient.GetAsync(new Id(club.Id!));
            if (existingClub == null)
            {
                await _clubClient.AddAsync(club);
            }
            else if (club != existingClub)
            {
                try
                {
                    await _clubClient.UpdateAsync(club with { ETag = existingClub.ETag });
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
            var existingAthlete = await _athleteClient.GetAsync(new Id(athlete.Id!));
            if (existingAthlete == null)
            {
                await _athleteClient.AddAsync(athlete);
            }
        }
    }
}