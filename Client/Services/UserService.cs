using System;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Services
{
    public class UserService
    {
        public event Action OnChange;

        private readonly StateContainerFactory _stateContainerFactory;
        private readonly IUserService _userClient;

        private User _user;

        public UserService(StateContainerFactory stateContainerFactory, IUserService userClient)
        {
            _stateContainerFactory = stateContainerFactory;
            _userClient = userClient;
        }

        public async Task LogIn(string identityReference)
        {
            // TODO: Get userId from identityReference when user authentication exists.
            _stateContainerFactory.CurrentUserId = "TestUser";
            _user = await _userClient.GetAsync(_stateContainerFactory.CurrentUserId);
            if (_user == null)
            {
                throw new Exception("Login failed: User not found :(");
            }
        }

        public Task LogOut()
        {
            _stateContainerFactory.CurrentUserId = null;
            _user = null;
            return Task.CompletedTask;
        }

        public async Task<User> GetUserAsync()
        {
            return _user ??= _stateContainerFactory.CurrentUserId != null
                ? await _userClient.GetAsync(_stateContainerFactory.CurrentUserId)
                : new User();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userClient.UpdateAsync(user);
            _user = user;
            OnChange?.Invoke();
        }

        public async Task FollowAthleteAsync(string athleteId, string name)
        {
            var subscription = new User.Subscription
            {
                AthleteId = athleteId,
                Name = name,
            };

            await FollowAsync(subscription);
        }

        public async Task FollowClubAsync(string clubId, string name)
        {
            var subscription = new User.Subscription
            {
                ClubId = clubId,
                Name = name,
            };

            await FollowAsync(subscription);
        }

        public async Task FollowAsync(User.Subscription subscription)
        {
            var user = await GetUserAsync();
            user.Subscriptions.Add(subscription);
            await UpdateUserAsync(user);
        }

        public bool IsFollowingClub(string clubId) =>
            _user != null && _user.Subscriptions.Any(s => s.ClubId == clubId);

        public bool IsFollowingAthlete(string athleteId) =>
            _user != null && _user.Subscriptions.Any(s => s.AthleteId == athleteId);
    }
}