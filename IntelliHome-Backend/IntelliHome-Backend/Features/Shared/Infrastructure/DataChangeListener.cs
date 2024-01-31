namespace IntelliHome_Backend.Features.Shared.Infrastructure
{
    public interface IDataChangeListener
    {
        void RegisterListener(Action<Guid> callback, Guid smartHomeId);
        void UnregisterListener(Action<Guid> callback, Guid smartHomeId);
        void HandleDataChange(Guid smartHomeId);
    }

    public class DataChangeListener : IDataChangeListener
    {
        private readonly Dictionary<Guid, List<Action<Guid>>> _listeners = new();

        public void RegisterListener(Action<Guid> callback, Guid smartHomeId)
        {
            if (!_listeners.ContainsKey(smartHomeId))
            {
                _listeners[smartHomeId] = new List<Action<Guid>>();
            }
            _listeners[smartHomeId].Add(callback);
        }

        public void UnregisterListener(Action<Guid> callback, Guid smartHomeId)
        {
            if (!_listeners.ContainsKey(smartHomeId)) return;
            _listeners[smartHomeId].Remove(callback);
            if (_listeners[smartHomeId].Count == 0)
            {
                _listeners.Remove(smartHomeId);
            }
        }

        public void HandleDataChange(Guid smartHomeId)
        {
            if (!_listeners.ContainsKey(smartHomeId)) return;
            foreach (var callback in _listeners[smartHomeId])
            {
                callback.Invoke(smartHomeId);
            }
        }
    }
}
