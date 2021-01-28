using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class SimpleEventSystem : MonoBehaviour
    {
        private static SimpleEventSystem _current;

        private Dictionary<string, Action> _listeners;

        public static void AddListener(string e, Action a)
        {
            if (ReferenceEquals(_current, null)) {
                Debug.LogError("the current scene has no defined SimpleEventSystem singleton");
                return;
            }
            if (_current._listeners.ContainsKey(e)) {
                Debug.LogErrorFormat("attempted to add listener to undefined event \"{0}\"", e);
                return;
            }

            _current._listeners[e] += a;
        }

        public static void RemoveListener(string e, Action a)
        {
            if (ReferenceEquals(_current, null)) {
                Debug.LogError("the current scene has no defined SimpleEventSystem singleton");
                return;
            }
            if (!_current._listeners.ContainsKey(e)) {
                Debug.LogErrorFormat("attempted to remove listener from undefined event \"{0}\"", e);
                return;
            }

            _current._listeners[e] -= a;
        }

        public static void TriggerEvent(string e) {
            if (ReferenceEquals(_current, null)) {
                Debug.LogError("the current scene has no defined SimpleEventSystem singleton");
                return;
            }
            if (!_current._listeners.ContainsKey(e)) {
                Debug.LogErrorFormat("attempted to trigger undefined event \"{0}\"", e);
                return;
            }
            if (_current._listeners[e] == default) {
                Debug.LogWarningFormat("attempted to trigger event \"{0}\" without listeners", e);
                return;
            }

            _current._listeners[e].Invoke();
        }

        private void Awake()
        {
            if (_current == null) {
                _current = this;
            } else if (_current != this) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            // only events defined in Types
            _listeners = new Dictionary<string, Action>();
            foreach (string e in Types.Events) {
                _listeners[e] = default;
            }
        }
    }
}
