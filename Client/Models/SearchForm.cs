using ScoreTracker.Shared;

namespace ScoreTracker.Client.Models;

public record SearchForm
{
    public StateCode? SelectedState { get; set; }
    public Discipline? SelectedDiscipline { get; set; }
    public int SelectedSeason { get; set; } = DateTime.Today.Year;
    public string? SearchString { get; set; }
}