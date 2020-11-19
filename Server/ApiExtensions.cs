using System;
using Google.Protobuf.WellKnownTypes;
using ScoreTracker.Server.Apis;
using ScoreTracker.Server.Services.Results;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Subscriptions;
using Discipline = ScoreTracker.Server.Services.Discipline;
using Enum = System.Enum;
using Meet = ScoreTracker.Server.Services.Meets.Meet;
using MeetQuery = ScoreTracker.Server.Services.Meets.MeetQuery;
using Result = ScoreTracker.Server.Services.Results.Result;
using Score = ScoreTracker.Server.Services.Results.Score;
using StateCode = ScoreTracker.Server.Services.StateCode;

namespace ScoreTracker.Server
{
    public static class ApiExtensions
    {
        public static MeetQuery ToServiceModel(this Shared.MeetQuery query)
        {
            var meetQuery = new MeetQuery
            {
                Discipline = Enum.Parse<Discipline>(query.Discipline.ToString()),
                StateCode = Enum.Parse<StateCode>(query.StateCode.ToString())
            };

            if (query.Name.Length > 0)
            {
                meetQuery.Name = query.Name;
            }

            if (query.City.Length > 0)
            {
                meetQuery.City = query.City;
            }

            if (query.StartDate?.Seconds > 0)
            {
                meetQuery.StartDate = query.StartDate.ToDateTime();
            }

            if (query.EndDate?.Seconds > 0)
            {
                meetQuery.EndDate = query.EndDate.ToDateTime();
            }

            if (query.Year > 0)
            {
                meetQuery.Year = query.Year;
            }

            return meetQuery;
        }

        public static Shared.Meet ToMeetApi(this Meet meet)
        {
            var meetApi = new Shared.Meet
            {
                Id = meet.Id,
                Name = meet.Name,
                Season = meet.Season,
                StartDate = meet.StartDate.ToUniversalTime().ToTimestamp(),
                EndDate = meet.EndDate.ToUniversalTime().ToTimestamp(),
                State = Enum.Parse<Shared.StateCode>(meet.State.ToString()),
                Discipline = Enum.Parse<Shared.Discipline>(meet.Discipline.ToString())
            };

            if (meet.Sessions != null)
            {
                foreach (var session in meet.Sessions)
                {
                    meetApi.Sessions.Add(new Shared.Session
                    {
                        Id = session.Id,
                        Name = session.Name
                    });
                }
            }

            if (meet.Levels != null)
            {
                foreach (var level in meet.Levels)
                {
                    var levelApi = new Shared.Level
                    {
                        Name = level.Name,
                        Abbreviation = level.Abbreviation ?? ""
                    };

                    if (level.Divisions != null)
                    {
                        foreach (var division in level.Divisions)
                        {
                            levelApi.Divisions.Add(new Shared.Division
                            {
                                Name = division.Name,
                                StartDate = division.StartDate.ToUniversalTime().ToTimestamp(),
                                EndDate = division.EndDate.ToUniversalTime().ToTimestamp()
                            });
                        }
                    }

                    meetApi.Levels.Add(levelApi);
                }
            }

            return meetApi;
        }

        public static Shared.Result ToResultApi(this Result result)
        {
            return new Shared.Result
            {
                Id = result.Id,
                Season = result.Season,
                MeetId = result.MeetId,
                Session = result.Session ?? "",
                Level = result.Level ?? "",
                AgeGroup = result.AgeGroup ?? "",
                MeetIdLevelDivision = result.MeetIdLevelDivision,
                AthleteId = result.AthleteId,
                AthleteName = result.AthleteName,
                Club = result.Club,
                ClubId = result.ClubId,
                Floor = result.Floor.ToScoreApi(),
                Horse = result.Horse.ToScoreApi(),
                Rings = result.Rings.ToScoreApi(),
                Vault = result.Vault.ToScoreApi(),
                PBars = result.PBars.ToScoreApi(),
                HBar = result.HBar.ToScoreApi(),
                AllAround = result.AllAround.ToScoreApi(),
            };
        }

        public static Shared.Score ToScoreApi(this Score score)
        {
            return new Shared.Score
            {
                EScore = score.EScore,
                DScore = score.DScore,
                FinalScore = score.FinalScore,
                NeutralDeductions = score.NeutralDeductions,
                Rank = score.Rank.GetValueOrDefault()
            };
        }

        public static Subscription ToServiceModel(this Proto.Subscription subscriptionApi) =>
            new Subscription
            {
                Id = subscriptionApi.Id,
                AthleteId = subscriptionApi.AthleteId,
                ClubId = subscriptionApi.ClubId,
                Name = subscriptionApi.Name,
                StartTime = subscriptionApi.StartTime?.ToDateTime() ?? DateTime.UtcNow
            };

        public static Proto.Subscription ToSubscriptionApi(this Subscription subscription) =>
            new Proto.Subscription
            {
                Id = subscription.Id,
                AthleteId = subscription.AthleteId,
                ClubId = subscription.ClubId,
                Name = subscription.Name,
                StartTime = subscription.StartTime.ToTimestamp()
            };

        public static SubscriptionQuery ToServiceModel(this Proto.SubscriptionQuery query) => new SubscriptionQuery();

        public static ResultsQuery ToResultsQuery(this ResultsRequest query) => new ResultsQuery(query.MeetId, query.Divisions);
    }
}