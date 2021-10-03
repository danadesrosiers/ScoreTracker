namespace ScoreTracker.Client;

public static class UserExtensions
{
    public static bool IsFollowingClub(this User? user, string clubId) =>
        user != null && user.Subscriptions.Any(s => s.ClubId == clubId);

    public static bool IsFollowingAthlete(
        this User? user, string athleteId, string? clubId = null, Discipline? discipline = null, string? level = null) =>
        user != null && user.Subscriptions.Any(
            s => s.AthleteId == athleteId || s.ClubId == clubId &&
                (s.Discipline == null || s.Discipline == discipline &&
                    (s.Level == null || s.Level == level)));
}