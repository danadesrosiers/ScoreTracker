using System.Collections.Generic;

namespace ScoreTracker.Server.Services.Results
{
    public record ResultsQuery(string MeetId, IEnumerable<string> Divisions = null);
}