using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server
{
    public class ResultNotificationService : BackgroundService
    {
        private readonly CosmosCollectionFactory _cosmosCollectionFactory;
        private readonly IAthleteService _athleteService;
        private readonly IMeetService _meetService;
        private readonly Dictionary<string, Meet> _cachedMeets = new();

        public ResultNotificationService(
            CosmosCollectionFactory cosmosCollectionFactory,
            IAthleteService athleteService,
            IMeetService meetService)
        {
            _cosmosCollectionFactory = cosmosCollectionFactory;
            _athleteService = athleteService;
            _meetService = meetService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var leaseContainer = await _cosmosCollectionFactory.GetLeaseContainer();
            var changeFeedProcessor = _cosmosCollectionFactory.GetContainer<MeetResult>()
                .GetChangeFeedProcessorBuilder<MeetResult>("resultNotificationFeed", HandleChangesAsync)
                .WithInstanceName("consoleHost")
                .WithLeaseContainer(leaseContainer)
                .Build();

            Console.WriteLine("Starting Change Feed Processor...");
            await changeFeedProcessor.StartAsync();
            Console.WriteLine("Change Feed Processor started.");

            await Task.Delay(-1, stoppingToken);
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<MeetResult> changes, CancellationToken cancellationToken)
        {
            Console.WriteLine("Started handling changes...");
            foreach (var result in changes)
            {
                Console.WriteLine($"Detected operation for item with id {result.Id}.");
                var athlete = await _athleteService.GetAsync(result.AthleteId);
                var existingScores = athlete.RecentScores?
                    .ToDictionary(rs => (rs.ResultId, rs.Event)) ?? new Dictionary<(string ResultId, Event Event), AthleteScore>();

                var meet = await GetMeet(result.MeetId);
                if (meet.Discipline == Discipline.Men)
                {
                    existingScores.AddUpdateRecentScore(result.Floor, Event.FX, result, meet);
                    existingScores.AddUpdateRecentScore(result.Horse, Event.PH, result, meet);
                    existingScores.AddUpdateRecentScore(result.Rings, Event.SR, result, meet);
                    existingScores.AddUpdateRecentScore(result.Vault, Event.VT, result, meet);
                    existingScores.AddUpdateRecentScore(result.PBars, Event.PB, result, meet);
                    existingScores.AddUpdateRecentScore(result.HBar,  Event.HB, result, meet);
                }

                // Sort by most recently modified.
                athlete.RecentScores = existingScores
                    .OrderBy(rs => rs.Value.Score.LastModified)
                    .Select(rs => rs.Value).ToList();

                await _athleteService.UpdateAsync(athlete);
            }

            Console.WriteLine("Finished handling changes.");
        }

        private async Task<Meet> GetMeet(string meetId)
        {
            if (!_cachedMeets.TryGetValue(meetId, out var meet))
            {
                meet = await _meetService.GetAsync(meetId);
                _cachedMeets[meetId] = meet;
            }

            return meet;
        }
    }

    internal static class DictionaryExtensions
    {
        public static void AddUpdateRecentScore(
            this IDictionary<(string, Event), AthleteScore> recentScores,
            Score score,
            Event eventEnum,
            MeetResult meetResult,
            Meet meet)
        {
            if (score == null) return;

            var key = (meetResult.Id, eventEnum);
            if (recentScores.TryGetValue(key, out var currentScore))
            {
                if (score.LastModified > currentScore.Score.LastModified)
                {
                    recentScores[key] = currentScore with { Score = score };
                }
            }
            else
            {
                recentScores[key] = new AthleteScore
                {
                    Discipline = meet.Discipline,
                    Event = eventEnum,
                    MeetId = meetResult.MeetId,
                    MeetName = meet.Name,
                    ResultId = meetResult.Id,
                    Score = score,
                    AthleteId = meetResult.AthleteId,
                    AthleteName = meetResult.AthleteName,
                    ClubId = meetResult.ClubId,
                    CLubName = meetResult.Club
                };
            }
        }
    }
}