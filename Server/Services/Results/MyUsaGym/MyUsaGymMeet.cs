using System;
using System.Collections.Generic;
using System.Linq;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;

namespace ScoreTracker.Server.Services.Results.MyUsaGym
{
    public class MyUsaGymMeet
    {
        public Sanction Sanction { get; set; }
        public Dictionary<int, MyUsaGymClub> Clubs { get; set; }
        public Dictionary<int, MyUsaGymPerson> People { get; set; }
        public List<MyUsaGymSession> Sessions { get; set; }
        public List<SessionResultSet> SessionResultSets { get; set; }

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
                Id = Sanction.SanctionId,
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
                    Id = person.PersonId,
                    ClubId = person.ClubId ?? default,
                    Name = person.FirstName + " " + person.LastName
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

    public class Sanction
    {
        public string SanctionId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StateCode State { get; set; }
        public Discipline DisciplineTypeId { get; set; }
    }

    public class SessionResultSet
    {
        public string SessionId { get; set; }
        public string Level { get; set; }
        public string Division { get; set; }
        public int ResultSetId { get; set; }
    }

    public class MyUsaGymSession
    {
        public string SessionId { get; set; }
        public int SanctionId { get; set; }
        public string Name { get; set; }
    }

    public class MyUsaGymPerson
    {
        public string PersonId { get; set; }
        public int? ClubId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MyUsaGymClub
    {
        public string ClubId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}