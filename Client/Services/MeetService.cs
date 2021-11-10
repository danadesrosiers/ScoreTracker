using ScoreTracker.Client.Services.RankStrategy;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Services;

public class MeetService : IMeetService
{
    private readonly IMeetClient _meetClient;
    private readonly IMeetResultClient _resultsClient;
    private readonly IRankStrategy _ranker;

    public MeetService(IMeetClient meetClient, IMeetResultClient resultsClient, IRankStrategy ranker)
    {
        _meetClient = meetClient;
        _resultsClient = resultsClient;
        _ranker = ranker;
    }

    public async Task<Meet?> GetMeetAsync(string meetId)
    {
        return await _meetClient.GetAsync(new Id(meetId));
    }

    public async Task<Dictionary<string, string>> SearchMeetsAsync(int selectedSeason, StateCode? selectedState,
        Discipline? selectedDiscipline, string? searchString)
    {
        var meetQuery = new MeetQuery
        {
            StateCode = selectedState,
            Year = selectedSeason,
            Discipline = selectedDiscipline,
            Name = searchString
        };
        return await _meetClient
            .GetAsync(meetQuery)
            .ToDictionaryAsync(m => m.Id!, m => m.Name);
    }

    public async Task<ICollection<Meet>> GetFollowingMeetsAsync(User user)
    {
        return await _meetClient.GetAsync(new MeetQuery()).ToListAsync();
    }

    public async Task<ICollection<MeetResult>> GetResults(string meetId, IEnumerable<string> divisions)
    {
        var timer = new Stopwatch();
        timer.Start();
        var query = new ResultsQuery { MeetId = meetId, Divisions = divisions };
        var results = await _resultsClient.GetAsync(query).ToListAsync();
        Console.WriteLine($"Found {results.Count} Results in {timer.ElapsedMilliseconds}ms.");
        return _ranker.AddRankings(results);
    }

    public ICollection<MeetResult> CalculateTeamResults(IEnumerable<MeetResult> results)
    {
        var timer = new Stopwatch();
        timer.Start();
        var scoresByClubLevel = new Dictionary<string, List<MeetResult>>();
        foreach (var result in results)
        {
            var key = result.ClubId + result.Level;
            if (!scoresByClubLevel.ContainsKey(key))
            {
                scoresByClubLevel[key] = new List<MeetResult>();
            }

            scoresByClubLevel[key].Add(result);
        }

        var teamScores = new List<MeetResult>();
        foreach (var (_, clubScores) in scoresByClubLevel)
        {
            var floorScore = (from result in clubScores
                orderby result.Floor?.FinalScore descending
                select result.Floor?.FinalScore ?? 0).Take(3).Sum();
            var horseScore = (from result in clubScores
                orderby result.Horse?.FinalScore descending
                select result.Horse?.FinalScore ?? 0).Take(3).Sum();
            var ringsScore = (from result in clubScores
                orderby result.Rings?.FinalScore descending
                select result.Rings?.FinalScore ?? 0).Take(3).Sum();
            var vaultScore = (from result in clubScores
                orderby result.Vault?.FinalScore descending
                select result.Vault?.FinalScore ?? 0).Take(3).Sum();
            var pBarsScore = (from result in clubScores
                orderby result.PBars?.FinalScore descending
                select result.PBars?.FinalScore ?? 0).Take(3).Sum();
            var hBarScore = (from result in clubScores
                orderby result.HBar?.FinalScore descending
                select result.HBar?.FinalScore ?? 0).Take(3).Sum();

            teamScores.Add(new MeetResult
            {
                Club = clubScores[0].Club,
                ClubId = clubScores[0].ClubId,
                Level = clubScores[0].Level,
                Floor = new Score(floorScore),
                Horse = new Score(horseScore),
                Rings = new Score(ringsScore),
                Vault = new Score(vaultScore),
                PBars = new Score(pBarsScore),
                HBar = new Score(hBarScore),
                AllAround = new Score(floorScore + horseScore + ringsScore + vaultScore + pBarsScore + hBarScore)
            });
        }

        Console.WriteLine($"Calculated Team Scores in {timer.ElapsedMilliseconds}ms.");

        return _ranker.AddRankings(teamScores);
    }
}