namespace ScoreTracker.Server.MeetResultsProviders.MyUsaGym;

public record MyUsaGymMeetSearchResult(
    string City,
    DateTime EndDate,
    int HasMeetInfo,
    int HasResults,
    string LogoUrl,
    string Name,
    Discipline Program,
    int SanctionId,
    string SiteName,
    DateTime StartDate,
    StateCode? State,
    string Website
);