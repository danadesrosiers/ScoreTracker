using System;
using System.Collections.Generic;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Services.Meets
{
    public class Meet : ICosmosEntity
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public int Season { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Level> Levels { get; set; }
        public StateCode? State { get; set; }
        public Discipline Discipline { get; set; }

        public bool IsLive()
        {
            return StartDate.Date >= DateTime.Now.Date && EndDate <= DateTime.Now.Date;
        }
    }

    public class Session
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Level
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public List<Division> Divisions { get; set; }
    }

    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}