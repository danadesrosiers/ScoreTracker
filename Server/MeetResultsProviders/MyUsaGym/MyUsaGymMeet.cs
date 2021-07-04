using System;
using System.Collections.Generic;
using System.Linq;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;

namespace ScoreTracker.Server.MeetResultsProviders.MyUsaGym
{
    public record MyUsaGymMeet(
        Sanction Sanction,
        Dictionary<string, MyUsaGymClub> Clubs,
        Dictionary<string, MyUsaGymPerson> People,
        IEnumerable<MyUsaGymSession> Sessions,
        IEnumerable<SessionResultSet> SessionResultSets)
    {
        public Meet GetMeet()
        {
            var divisionsByLevel = new Dictionary<string, List<string>>();
            foreach (var resultSet in SessionResultSets)
            {
                if (!divisionsByLevel.ContainsKey(resultSet.Level))
                {
                    divisionsByLevel[resultSet.Level] = new List<string>();
                }
                divisionsByLevel[resultSet.Level].Add(resultSet.Division);
            }

            var levels =
                (from ld in divisionsByLevel
                let levelName = ld.Key
                let divisionNames = ld.Value
                select new Level
                {
                    Name = levelName
                }).ToList();

            foreach (var level in levels)
            {
                level.Divisions = (
                    from divisions in divisionsByLevel[level.Name].Distinct()
                    select new Division { Name = divisions }).ToList();
            }

            return new Meet
            {
                Id = Sanction.SanctionId.ToString(),
                Name = Sanction.Name,
                StartDate = Sanction.StartDate.ToUniversalTime(),
                EndDate = Sanction.EndDate.ToUniversalTime(),
                Season = Sanction.StartDate.ToUniversalTime().Year,
                State = Sanction.State,
                Discipline = Sanction.DisciplineTypeId,
                Levels = levels
            };
        }

        public List<Athlete> GetAthletes()
        {
            return (from person in People.Values
                select new Athlete
                {
                    Id = person.PersonId
                }).ToList();
        }

        public List<Club> GetClubs()
        {
            return (from club in Clubs.Values
                select new Club
                {
                    Id = club.ClubId,
                    Name = club.Name,
                    ShortName = club.ShortName,
                }).ToList();
        }
    }

    public record Sanction(int SanctionId, string Name, DateTime StartDate, DateTime EndDate, StateCode State,
        Discipline DisciplineTypeId);

    public record SessionResultSet(string SessionId, string Level, string Division, int ResultSetId)
    {
        public string GetLevel()
        {
            var levels = Level.Split(',').Select(l => l.Trim()[0].ToString()).Distinct().ToList();
            return levels.Count == 1 ? levels.First() : "Unknown";
        }
    }

    public record MyUsaGymSession(string SessionId, int SanctionId, string Name);

    public record MyUsaGymPerson(string PersonId, int? ClubId, string FirstName, string LastName);

    public record MyUsaGymClub(string ClubId, string Name, string? ShortName);
}