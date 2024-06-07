#if EVENTBROKER_UNITASK_ENABLED
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace OsirisGames.EventBroker
{
    public interface IEventBusAsync
    {
        void Subscribe<T>(Func<T, UniTask> action);
        UniTask Fire<T>(T signal, CancellationToken token = default);
        void Unsubscribe<T>(Func<T, UniTask> action);
    }
}
#endif