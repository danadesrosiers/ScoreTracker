namespace ScoreTracker.Server.MeetResultsProviders.MyUsaGym;

public class MeetInfo
{
    public Meet Meet { get; init; } = null!;
    public List<Athlete> Athletes { get; init; } = new();
    public List<Club> Clubs { get; init; } = new();
    public List<MeetResult> Results { get; init; } = new();
}