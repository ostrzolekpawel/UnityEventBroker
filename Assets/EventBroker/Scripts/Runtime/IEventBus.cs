using System;

namespace OsirisGames.EventBroker
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> action);
        void Fire<T>(T signal);
        void Unsubscribe<T>(Action<T> action);
    }
}
