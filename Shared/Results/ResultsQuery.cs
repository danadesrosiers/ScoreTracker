using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<Result> ConfigureQuery(IQueryable<Result> queryable)
        {
            var query = queryable.Where(result => result.MeetId == MeetId);

            var meetLevelDivisions = Divisions?.Select(div => $"{MeetId}{div}");
            if (meetLevelDivisions != null)
            {
                query = query.Where(result => meetLevelDivisions.Contains(result.MeetIdLevelDivision));
            }

            query = query.OrderByDescending(result => result.AllAround.FinalScore);

            if (Limit != null)
            {
                query = query.Take(Limit.Value);
            }

            return query;
        }
    }
}