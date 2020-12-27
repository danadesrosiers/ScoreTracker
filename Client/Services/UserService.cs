using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Services
{
    public class UserService
    {
        public event Action? OnUserChange;

        private readonly StateContainerFactory _stateContainerFactory;
        private readonly IUserService _userClient;

        public UserService(StateContainerFactory stateContainerFactory, IUserService userClient)
        {
            _stateContainerFactory = stateContainerFactory;
            _userClient = userClient;
        }

        public User? User { get; set; }

        public async Task LogIn(string identityReference)
        {
            // TODO: Get userId from identityReference when user authentication exists.
            _stateContainerFactory.CurrentUserId = "TestUser";
            User = await _userClient.GetAsync(_stateContainerFactory.CurrentUserId);
            Console.WriteLine(JsonSerializer.Serialize(OnUserChange?.GetInvocationList().Select(x => x.Target?.GetType().Name)));
            OnUserChange?.Invoke();
            if (User == null)
            {
                throw new Exception("Login failed: User not found :(");
            }
        }

        public Task LogOut()
        {
            _stateContainerFactory.CurrentUserId = null;
            User = null;
            OnUserChange?.Invoke();
            return Task.CompletedTask;
        }

        public async Task<User?> GetUserAsync()
        {
            return User ??= _stateContainerFactory.CurrentUserId != null
                ? await _userClient.GetAsync(_stateContainerFactory.CurrentUserId)
                : new User();
        }

        public async Task UpdateUserAsync(User user)
        {
            User = await _userClient.UpdateAsync(user);
            OnUserChange?.Invoke();
        }

        public async Task FollowAthleteAsync(string athleteId, string name)
        {
            var subscription = new Subscription
            {
                AthleteId = athleteId,
                Name = name,
            };

            await FollowAsync(subscription);
        }

        public async Task FollowClubAsync(string clubId, string name)
        {
            var subscription = new Subscription
            {
                ClubId = clubId,
                Name = name,
            };

            await FollowAsync(subscription);
        }

        public async Task FollowAsync(Subscription subscription)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                user.Subscriptions.Add(subscription);
                await UpdateUserAsync(user);
            }
        }

        public bool IsFollowingClub(string clubId) =>
            User != null && User.Subscriptions.Any(s => s.ClubId == clubId);

        public bool IsFollowingAthlete(string athleteId) =>
            User != null && User.Subscriptions.Any(s => s.AthleteId == athleteId);

        public bool IsLoggedIn()
        {
            return User != null;
        }
    }
}