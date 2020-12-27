using System.Collections.Concurrent;

namespace ScoreTracker.Client
{
    public class StateContainerFactory
    {
        private readonly ConcurrentDictionary<string, StateContainer> _stateContainers = new();
        private StateContainer? _guestUser;

        public string? CurrentUserId { get; set; }

        public StateContainer GetState()
        {
            if (CurrentUserId == null)
            {
                return _guestUser ??= new StateContainer();
            }

            _guestUser = null;
            return _stateContainers.GetOrAdd(CurrentUserId, _ => new StateContainer());
        }
    }
}