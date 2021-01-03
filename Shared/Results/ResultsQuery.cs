using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Results
{
    [DataContract]
    public record ResultsQuery : IQuery<MeetResult, MeetResult>
    {
        [DataMember(Order = 1)]
        public string? MeetId { get; init; }
        [DataMember(Order = 2)]
        public IEnumerable<string>? Divisions { get; init; }
        [DataMember(Order = 3)]
        public MeetResultOrderBy? OrderBy { get; init; }
        public int? Limit { get; init; }

        public IQueryable<MeetResult> ConfigureQuery(IQueryable<MeetResult> queryable)
        {
            var query = queryable.Where(result => result.MeetId == MeetId);

            var meetLevelDivisions = Divisions?.Select(div => $"{MeetId}{div}");
            if (meetLevelDivisions != null)
            {
                query = query.Where(result => meetLevelDivisions.Contains(result.MeetIdLevelDivision));
            }

            query = OrderBy switch
            {
                MeetResultOrderBy.AllAround => query.OrderByDescending(result => result.AllAround.FinalScore),
                MeetResultOrderBy.LastModified => query.OrderByDescending(result => result.LastUpdated),
                _ => query
            };

            if (Limit != null)
            {
                query = query.Take(Limit.Value);
            }

            return query;
        }
    }

    public enum MeetResultOrderBy
    {
        AllAround,
        LastModified
    }
}