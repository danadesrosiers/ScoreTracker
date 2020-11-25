using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Server.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ICosmosRepository<User> _userRepository;

        public UserService(ICosmosRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetAsync(string userId)
        {
            return await _userRepository.GetAsync(userId);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }
    }
}