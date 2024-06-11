using Cysharp.Threading.Tasks;
using NUnit.Framework;
using OsirisGames.EventBroker;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TestTools;

public class EventBrokerAsyncFireTest
{
    [UnityTest]
    public IEnumerator Fire_DoesNotInvokeUnrelatedSubscriptions() => UniTask.ToCoroutine(async () =>
    {
            // Arrange
            var eventBus = new EventBusAsync();
            bool eventFired = false;
            Func<string, UniTask> action = (message) =>
            {
                eventFired = true;
                return UniTask.FromResult(message);
            };

            eventBus.Subscribe<int>((value) => UniTask.CompletedTask);

            // Act
            eventBus.Subscribe(action);
            await eventBus.Fire("Test Event");

            // Assert
            Assert.IsTrue(eventFired);
    });

    [UnityTest]
    public IEnumerator Fire_InvokesAllSubscribedActions_For_SpecificEventType() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new EventBusAsync();
        int counter = 0;
        Func<int, UniTask> action1 = (num) =>
        {
            counter += num;
            return UniTask.FromResult(counter);
        };
        Func<int, UniTask> action2 = (num) => 
        { 
            counter += num * 2;
            return UniTask.FromResult(counter);
        };

        // Act
        eventBus.Subscribe(action1);
        eventBus.Subscribe(action2);
        await eventBus.Fire(5);

        // Assert
        Assert.AreEqual(15, counter);
    });

    [UnityTest]
    public IEnumerator Fire_MultipleSubscriptions_ForSameEventType_Is_HandledCorrectly() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new EventBusAsync();
        int eventCount = 0;
        Func<int, UniTask> action = (number) =>
        {
            eventCount += number;
            return UniTask.FromResult(eventCount);
        };

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        await eventBus.Fire(5);

        // Assert
        Assert.AreEqual(10, eventCount);
    });

    [Test]
    public void Fire_DoesNothing_If_NoSubscriptionsExist()
    {
        // Arrange
        var eventBus = new EventBusAsync();

        // Act && Assert
        Assert.DoesNotThrow(() => eventBus.Fire("Test Event").Forget());
    }

    [UnityTest]
    public IEnumerator Fire_WithNull_DoesNotCause_UnexpectedBehavior() => UniTask.ToCoroutine(async () =>
    {
            // Arrange
            var eventBus = new EventBusAsync();
            bool eventFired = false;
            Func<string, UniTask> action = (message) =>
            {
                eventFired = true;
                return UniTask.FromResult(message);
            };

            // Act
            eventBus.Subscribe(action);
            await eventBus.Fire<string>(null);

            // Assert
            Assert.IsTrue(eventFired);
    });

    [UnityTest]
    public IEnumerator Fire_WillFinish_AfterLongestAction() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new EventBusAsync();

        Func<string, UniTask> action1 = async (message) =>
        {
            await UniTask.Delay(1000);
        };

        Func<string, UniTask> action2 = async (message) =>
        {
            await UniTask.Delay(2000);
        };

        // Act
        eventBus.Subscribe(action1);
        eventBus.Subscribe(action2);

        var stopwatch = Stopwatch.StartNew();
        await eventBus.Fire<string>(null);
        stopwatch.Stop();

        // Assert
        Assert.True(Mathf.Abs(stopwatch.ElapsedMilliseconds - 2000) < 100);
    });

    [UnityTest]
    public IEnumerator Fire_WillFinish_AfterLongestAction2() => UniTask.ToCoroutine(async () =>
    {
        // Arrange
        var eventBus = new EventBusAsync();

        Func<string, UniTask> action1 = async (message) =>
        {
            await UniTask.Delay(2000);
        };

        Func<string, UniTask> action2 = async (message) =>
        {
            await UniTask.Delay(1000);
        };

        // Act
        eventBus.Subscribe(action1);
        eventBus.Subscribe(action2);

        var stopwatch = Stopwatch.StartNew();
        await eventBus.Fire<string>(null);
        stopwatch.Stop();

        // Assert
        Assert.True(Mathf.Abs(stopwatch.ElapsedMilliseconds - 2000) < 100);
    });
}
