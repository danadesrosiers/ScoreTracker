using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Results
{
    [DataContract]
    public record ResultsQuery
    {
        [DataMember(Order = 1)]
        public string MeetId { get; init; }
        [DataMember(Order = 2)]
        public IEnumerable<string> Divisions { get; init; }
        [DataMember(Order = 3)]
        public int? Limit { get; init; }
    }
}