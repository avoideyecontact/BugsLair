using System;

public interface IEventBus
{
    void Publish<T>(T e) where T : struct;
    void Subscribe<T>(Action<T> handler) where T : struct;
    void Unsubscribe<T>(Action<T> handler) where T : struct;
}
