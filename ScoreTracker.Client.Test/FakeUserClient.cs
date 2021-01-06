using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Test
{
    public class FakeUserClient : IUserClient
    {
        private readonly Dictionary<Id, User> _users = new();

        public IAsyncEnumerable<TResult> GetAsync<TResult>(IQuery<User, TResult> query)
        {
            return new List<TResult>().ToAsyncEnumerable();
        }

        public Task<User?> GetAsync(Id id)
        {
            return Task.FromResult<User?>(_users[id]);
        }

        public Task<User> AddAsync(User user)
        {
            _users[new Id(user.Id!)] = user;
            return Task.FromResult(user);
        }

        public Task<User> UpdateAsync(User user)
        {
            _users[new Id(user.Id!)] = user;
            return Task.FromResult(user);
        }

        public Task DeleteAsync(Id id)
        {
            _users.Remove(id);
            return Task.CompletedTask;
        }
    }
}