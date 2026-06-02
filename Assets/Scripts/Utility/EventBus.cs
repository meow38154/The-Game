using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _handlers = new();

        public static void Subscribe<T>(Action<T> handle)
        {
            _handlers.TryGetValue(typeof(T), out var existing);
            _handlers[typeof(T)] = (Action<T>)existing + handle;
        }

        public static void Unsubscribe<T>(Action<T> handle)
        {
            if (_handlers.TryGetValue(typeof(T), out var existing))
                _handlers[typeof(T)] = (Action<T>)existing - handle;
        }

        public static void Publish<T>(T message)
        {
            if (_handlers.TryGetValue(typeof(T), out var existing))
                ((Action<T>)existing)?.Invoke(message);
        }
    }
}