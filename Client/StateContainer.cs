using System;
using ScoreTracker.Shared;

namespace ScoreTracker.Client
{
    public class StateContainer
    {
        public int SelectedSeason { get; set; } = DateTime.Today.Year;
        public StateCode? SelectedState { get; set; }
        public Discipline? SelectedDiscipline { get; set; }

        public StateDictionary<string, StateDictionary<string, bool>> SelectedLevels { get; } = new();
        public StateDictionary<string, StateDictionary<string, bool>> SelectedDivisions { get; } = new();
    }
}