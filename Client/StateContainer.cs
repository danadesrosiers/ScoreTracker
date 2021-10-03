namespace ScoreTracker.Client;

public class StateContainer
{
    public SearchForm SearchForm { get; } = new();

    public StateDictionary<string, StateDictionary<string, bool>> SelectedLevels { get; } = new();
    public StateDictionary<string, StateDictionary<string, bool>> SelectedDivisions { get; } = new();
}