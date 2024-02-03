using Data.Models.Home;

namespace IntelliHome_Backend.Features.Shared.Infrastructure
{
    public interface IDataChangeListener
    {
        void RegisterListener(Action<string> callback, string smartHomeId);
        void UnregisterListener(Action<string> callback, string smartHomeId);
        void HandleDataChange(string smartHomeId);
    }

    public class DataChangeListener : IDataChangeListener
    {
        private readonly Dictionary<string, List<Action<string>>> _listeners = new();

        public void RegisterListener(Action<string> callback, string smartHomeId)
        {
            if (string.IsNullOrEmpty(smartHomeId))
            {
                return;
            }

            if (!_listeners.ContainsKey(smartHomeId))
            {
                _listeners[smartHomeId] = new List<Action<string>>();
            }
            else
            {
                if (_listeners[smartHomeId].Contains(callback))
                {
                    return;
                }
            }
            _listeners[smartHomeId].Add(callback);
        }

        public void UnregisterListener(Action<string> callback, string smartHomeId)
        {
            if (!_listeners.ContainsKey(smartHomeId)) return;
            _listeners[smartHomeId].Remove(callback);
            if (_listeners[smartHomeId].Count == 0)
            {
                _listeners.Remove(smartHomeId);
            }
        }

        public void HandleDataChange(string smartHomeId)
        {
            // give me all listeners keys
            var keys = _listeners.Keys.ToList();
            var filteredKeys = keys.Where(key => key.Contains(smartHomeId)).ToList();
            if (filteredKeys.Count == 0) return;
            foreach (var key in filteredKeys)
            {
                foreach (var listener in _listeners[key])
                {
                    listener.Invoke(key);
                }
            }
        }
    }
}
