using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Users
{
    [ServiceContract]
    public interface IUserService
    {
        Task<User?> GetAsync(string userId);

        Task<User> UpdateAsync(User user);
    }
}