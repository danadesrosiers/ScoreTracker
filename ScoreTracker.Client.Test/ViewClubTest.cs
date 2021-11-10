using ScoreTracker.Client.Pages;
using ScoreTracker.Client.Services;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Test;

public class ViewClubTest : TestContext
{
    private readonly IAthleteClient _athleteClient;
    private readonly IClubClient _clubClient;

    public ViewClubTest()
    {
        IUserClient userClient = new FakeUserClient();
        _athleteClient = new FakeAthleteClient();
        _clubClient = new FakeClubClient();

        Services.AddSingleton<IUserService>(new UserService(new StateContainerFactory(), userClient));
        Services.AddSingleton<IAthleteService>(new AthleteService(_athleteClient));
        Services.AddSingleton<IClubService>(new ClubService(_clubClient));
    }
    [Fact]
    public async Task HasListOfAthletesByDisciplineAndLevel()
    {
        await _clubClient.AddAsync(new Club {Id = "1234", Name = "Test Club"});
        await _athleteClient.AddAsync(new Athlete
        {
            Id = "1",
            RecentScores = new List<AthleteResult>
            {
                new() { ClubId = "1234", AthleteId = "1", AthleteName = "One", Discipline = Discipline.Men, Level = "5", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "2", AthleteName = "Two", Discipline = Discipline.Men, Level = "5", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "3", AthleteName = "Three", Discipline = Discipline.Men, Level = "5", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "4", AthleteName = "Four", Discipline = Discipline.Men, Level = "6", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "5", AthleteName = "Five", Discipline = Discipline.Men, Level = "6", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "6", AthleteName = "Six", Discipline = Discipline.Men, Level = "7", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "7", AthleteName = "Seven", Discipline = Discipline.Men, Level = "8", Score = new Score(0), MeetId = "1" },
                new() { ClubId = "1234", AthleteId = "8", AthleteName = "Eight", Discipline = Discipline.Men, Level = "9", Score = new Score(0), MeetId = "1" },
            }
        });
        var cut = RenderComponent<ViewClub>(parameters => parameters
            .Add(p => p.ClubId, "1234"));

        cut.Instance.Teams.Should().ContainKey(Discipline.Men);
        cut.Instance.Teams[Discipline.Men].Should().ContainKey("5");
        cut.Instance.Teams[Discipline.Men]["5"].Count.Should().Be(3);
    }
}