using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OsirisGames.EventBroker
{
    public class EventBusAsync : IEventBusAsync
    {
        private readonly Dictionary<Type, List<Delegate>> _subscriptions = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Func<T, UniTask> action)
        {
            Type type = typeof(T);
            if (!_subscriptions.ContainsKey(type))
            {
                _subscriptions[type] = new List<Delegate>();
            }

            _subscriptions[type].Add(action);
        }

        public async UniTask Fire<T>(T signal, CancellationToken token = default)
        {
            Type type = typeof(T);
            if (_subscriptions.ContainsKey(type))
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                var tasks = _subscriptions[type]
                            .OfType<Func<T, UniTask>>()
                            .Select(subscription => subscription(signal))
                            .ToList();

                await UniTask.WhenAll(tasks).AttachExternalCancellation(token);
            }
        }

        public void Unsubscribe<T>(Func<T, UniTask> action)
        {
            Type type = typeof(T);
            if (_subscriptions.ContainsKey(type))
            {
                _subscriptions[type].Remove(action);
            }
        }
    }
}
