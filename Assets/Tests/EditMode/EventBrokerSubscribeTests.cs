﻿using NUnit.Framework;
using OsirisGames.EventBroker;
using System;

public class EventBrokerSubscribeTests
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
}
