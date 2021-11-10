using ScoreTracker.Shared;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Test;

public class UserExtensionsTest
{
    private readonly User _user;

    public UserExtensionsTest()
    {
        _user = new User
        {
            Id = "TestUser",
            Name = "Test User",
            Subscriptions = new List<Subscription>
            {
                new() {AthleteId = "athlete"},
                new() {ClubId = "club"},
                new() {ClubId = "club2", Discipline = Discipline.Men},
                new() {ClubId = "club3", Discipline = Discipline.Men, Level = "5"}
            }
        };
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeFalse_WhenNotLoggedIn()
    {
        (null as User).IsFollowingAthlete("athlete", "club", Discipline.Men, "5").Should().BeFalse();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeTrue_WhenSubscribedToAthlete()
    {
        _user.IsFollowingAthlete("athlete", "club4", Discipline.Men, "5").Should().BeTrue();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeFalse_WhenNotSubscribedToAthleteOrClub()
    {
        _user.IsFollowingAthlete("athlete2", "club4", Discipline.Men, "5").Should().BeFalse();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeTrue_WhenSubscribedToClub()
    {
        _user.IsFollowingAthlete("athlete2", "club", Discipline.Women, "6").Should().BeTrue();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeTrue_WhenSubscribedToClubDiscipline()
    {
        _user.IsFollowingAthlete("athlete2", "club2", Discipline.Men, "6").Should().BeTrue();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeFalse_WhenNotSubscribedToClubDiscipline()
    {
        _user.IsFollowingAthlete("athlete2", "club2", Discipline.Women, "6").Should().BeFalse();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeTrue_WhenSubscribedToClubDisciplineLevel()
    {
        _user.IsFollowingAthlete("athlete2", "club3", Discipline.Men, "5").Should().BeTrue();
    }

    [Fact]
    public void IsFollowingAthlete_ShouldBeFalse_WhenNotSubscribedToClubDisciplineLevel()
    {
        _user.IsFollowingAthlete("athlete2", "club3", Discipline.Men, "6").Should().BeFalse();
    }
}