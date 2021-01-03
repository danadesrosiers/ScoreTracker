using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Athletes
{
    [DataContract]
    public record AthleteResultQuery : IQuery<Athlete, AthleteResult>
    {
        [DataMember(Order = 1)]
        public IEnumerable<string> ClubIds { get; init; } = new List<string>();

        [DataMember(Order = 2)]
        public IEnumerable<string> AthleteIds { get; init; } = new List<string>();

        public IQueryable<AthleteResult> ConfigureQuery(IQueryable<Athlete> queryable)
        {
            return queryable
                .SelectMany(athlete => athlete.RecentScores)
                .Where(score => ClubIds.Contains(score.ClubId) || AthleteIds.Contains(score.AthleteId));
        }
    }
}