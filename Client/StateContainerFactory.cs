using System.Collections.Generic;

namespace ScoreTracker.Client
{
    public class StateContainerFactory
    {
        private readonly Dictionary<string, StateContainer> _stateContainers = new();
        private StateContainer? _guestUser;

        public string? CurrentUserId { get; set; }

        public StateContainer GetState()
        {
            if (CurrentUserId == null)
            {
                return _guestUser ??= new StateContainer();
            }

            _guestUser = null;
            if (!_stateContainers.TryGetValue(CurrentUserId, out var stateContainer))
            {
                stateContainer = new StateContainer();
                _stateContainers[CurrentUserId] = stateContainer;
            }
            return stateContainer;
        }
    }
}