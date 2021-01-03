using System.ServiceModel;

namespace ScoreTracker.Shared.Clubs
{
    [ServiceContract]
    public interface IClubClient : IClient<Club>
    {
    }
}