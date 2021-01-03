using System;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.MeetResultsProviders.MyUsaGym
{
    public record MyUsaGymMeetSearchResult(
        string City,
        DateTime EndDate,
        bool HasMeetInfo,
        bool HasResults,
        string LogoUrl,
        string Name,
        Discipline Program,
        string SanctionId,
        string SiteName,
        DateTime StartDate,
        StateCode? State,
        string Website
    );
}