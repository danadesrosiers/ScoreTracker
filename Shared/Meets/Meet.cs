using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Meets
{
    [DataContract]
    public record Meet : CosmosEntity
    {
        [DataMember(Order = 1)]
        public override string? Id { get; init; }
        [DataMember(Order = 2)]
        public string Name { get; init; } = null!;
        [DataMember(Order = 3)]
        public int Season { get; init; }
        [DataMember(Order = 4)]
        public DateTime StartDate { get; init; }
        [DataMember(Order = 5)]
        public DateTime EndDate { get; init; }
        [DataMember(Order = 6)]
        public List<Session> Sessions { get; init; } = new();
        [DataMember(Order = 7)]
        public List<Level> Levels { get; init; } = new();
        [DataMember(Order = 8)]
        public StateCode? State { get; init; }
        [DataMember(Order = 9)]
        public Discipline Discipline { get; init; }
        [DataMember(Order = 10)]
        public override string? ETag { get; init; }

        public bool IsLive()
        {
            return StartDate.Date >= DateTime.Now.Date && EndDate <= DateTime.Now.Date;
        }
    }

    [DataContract]
    public class Session
    {
        [DataMember(Order = 1)]
        public string Id { get; init; } = null!;
        [DataMember(Order = 2)]
        public string Name { get; init; } = null!;
    }

    [DataContract]
    public class Level
    {
        [DataMember(Order = 1)]
        public string Name { get; init; } = null!;
        [DataMember(Order = 2)]
        public string? Abbreviation { get; init; }
        [DataMember(Order = 3)]
        public List<Division> Divisions { get; set; } = new();
    }

    [DataContract]
    public class Division
    {
        [DataMember(Order = 1)]
        public int Id { get; init; }
        [DataMember(Order = 2)]
        public string Name { get; init; } = null!;
        [DataMember(Order = 3)]
        public DateTime StartDate { get; init; }
        [DataMember(Order = 4)]
        public DateTime EndDate { get; init; }
    }
}