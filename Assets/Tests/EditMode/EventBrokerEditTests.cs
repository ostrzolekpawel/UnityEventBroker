using NUnit.Framework;
using OsirisGames.EventBroker;
using System;

public class EventBrokerEditTests
{
    [Test]
    public void Subscribe_AddsNewSubscription_For_EventType()
    {
        // Arrange
        var eventBus = new EventBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire("Test Event");

        // Assert
        Assert.IsTrue(eventFired);
    }

    [Test]
    public void Subscribe_WithNull_ThrowsException()
    {
        // Arrange
        var eventBus = new EventBus();

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => eventBus.Subscribe<string>(null));
    }

    [Test]
    public void Unsubscribe_RemovesCorrectSubscription()
    {
        // Arrange
        var eventBus = new EventBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire("Test Event");
        eventBus.Unsubscribe(action);
        eventFired = false;
        eventBus.Fire("Test Event");

        // Assert
        Assert.IsFalse(eventFired);
    }

    [Test]
    public void Fire_InvokesAllSubscribedActions_For_SpecificEventType()
    {
        // Arrange
        var eventBus = new EventBus();
        int counter = 0;
        Action<int> action1 = (num) => counter += num;
        Action<int> action2 = (num) => counter += num * 2;

        // Act
        eventBus.Subscribe(action1);
        eventBus.Subscribe(action2);
        eventBus.Fire(5);

        // Assert
        Assert.AreEqual(15, counter);
    }

    [Test]
    public void Fire_MultipleSubscriptions_ForSameEventType_Is_HandledCorrectly()
    {
        // Arrange
        var eventBus = new EventBus();
        int eventCount = 0;
        Action<int> action = (number) => eventCount += number;

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Fire(5);

        // Assert
        Assert.AreEqual(10, eventCount);
    }

    [Test]
    public void Fire_DoesNothing_If_NoSubscriptionsExist()
    {
        // Arrange
        var eventBus = new EventBus();

        // Act && Assert
        Assert.DoesNotThrow(() => eventBus.Fire("Test Event"));
    }

    [Test]
    public void Fire_WithNull_DoesNotCause_UnexpectedBehavior()
    {
        // Arrange
        var eventBus = new EventBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire<string>(null);

        // Assert
        Assert.IsTrue(eventFired);
    }

    [Test]
    public void Unsubscribe_WithNull_ThrowsException()
    {
        // Arrange
        var eventBus = new EventBus();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => eventBus.Unsubscribe<string>(null));
    }

    [Test]
    public void Unsubscribe_ForNonExistentSubscription_DoesNot_CauseUnexpectedBehavior()
    {
        // Arrange
        var eventBus = new EventBus();
        Action<string> action = (message) => { };

        // Act && Assert
        Assert.DoesNotThrow(() => eventBus.Unsubscribe(action));
    }

    [Test]
    public void Subscribe_SameActionMultipleTimes_ResultsIn_MultipleInvocations()
    {
        // Arrange
        var eventBus = new EventBus();
        int invocationCount = 0;
        Action<string> action = (message) => invocationCount++;

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Fire("Test Event");

        // Assert
        Assert.AreEqual(3, invocationCount);
    }

    [Test]
    public void Unsubscribe_RemovesOneInstance_Of_IdenticalAction()
    {
        // Arrange
        var eventBus = new EventBus();
        int eventCount = 0;
        Action<int> action = (number) => eventCount++;

        // Act
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Subscribe(action);
        eventBus.Unsubscribe(action);
        eventBus.Fire(5);

        // Assert
        Assert.AreEqual(2, eventCount);
    }

    [Test]
    public void Fire_DoesNotInvokeUnrelatedSubscriptions()
    {
        // Arrange
        var eventBus = new EventBus();
        bool eventFired = false;
        Action<string> action = (message) => eventFired = true;

        eventBus.Subscribe<int>((value) => { });

        // Act
        eventBus.Subscribe(action);
        eventBus.Fire("Test Event");

        // Assert
        Assert.IsTrue(eventFired);
    }
}
