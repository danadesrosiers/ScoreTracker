using System.ServiceModel;

namespace ScoreTracker.Shared.Users
{
    [ServiceContract]
    public interface IUserClient : IClient<User>
    {
    }
}