using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Meets
{
    [DataContract]
    public record Meet : ICosmosEntity
    {
        [DataMember(Order = 1)]
        public string Id { get; init; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public int Season { get; set; }
        [DataMember(Order = 4)]
        public DateTime StartDate { get; set; }
        [DataMember(Order = 5)]
        public DateTime EndDate { get; set; }
        [DataMember(Order = 6)]
        public List<Session> Sessions { get; set; }
        [DataMember(Order = 7)]
        public List<Level> Levels { get; set; }
        [DataMember(Order = 8)]
        public StateCode? State { get; set; }
        [DataMember(Order = 9)]
        public Discipline Discipline { get; set; }

        public bool IsLive()
        {
            return StartDate.Date >= DateTime.Now.Date && EndDate <= DateTime.Now.Date;
        }
    }

    [DataContract]
    public class Session
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
    }

    [DataContract]
    public class Level
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 2)]
        public string Abbreviation { get; set; }
        [DataMember(Order = 3)]
        public List<Division> Divisions { get; set; }
    }

    [DataContract]
    public class Division
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public DateTime StartDate { get; set; }
        [DataMember(Order = 4)]
        public DateTime EndDate { get; set; }
    }
}