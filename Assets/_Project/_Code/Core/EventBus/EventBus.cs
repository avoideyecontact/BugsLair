using System;
using System.Collections.Generic;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, Delegate> _handlers = new Dictionary<Type, Delegate>();

    public void Publish<T>(T e) where T : struct
    {
        if (_handlers.TryGetValue(typeof(T), out var del))
        {
            (del as Action<T>)?.Invoke(e);
        }
    }

    public void Subscribe<T>(Action<T> handler) where T : struct
    {
        Type type = typeof(T);
        if (_handlers.ContainsKey(type))
        {
            _handlers[type] = Delegate.Combine(_handlers[type], handler);
        }
        else
        {
            _handlers[type] = handler;
        }
    }

    public void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        Type type = typeof(T);
        if (_handlers.ContainsKey(type))
        {
            _handlers[type] = Delegate.Remove(_handlers[type], handler);
        }
    }
}
