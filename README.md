# Unity Event Broker

## Installation

There is several options to install this package:
- UPM
- directly in manifest

### Unity Package Manager

Open Unity Package Manager and go to **Add package from git URL...** and paste [https://github.com/ostrzolekpawel/UnityEventBroker.git?path=Assets/EventBroker/Assets](https://github.com/ostrzolekpawel/UnityEventBroker.git?path=Assets/EventBroker/Assets)

### Manifest
Add link to package from repository directly to manifest.json

**Example**
```json
{
    "dependencies": {
        // other packages
        // ...
        "com.osirisgames.unityeventbroker": "https://github.com/ostrzolekpawel/UnityEventBroker.git?path=Assets/EventBroker/Assets"
    }
}
```

## Introduction

Simple implementation of `Signal Bus` which allows sync and async calls:
- `EventBus` - sync calls
- `EventBusAsync` - async calls using [UniTask](https://github.com/Cysharp/UniTask)

Plugin also provides interfaces for custom implentations.

### IEventBus

```cs
public interface IEventBus
{
    void Subscribe<T>(Action<T> action);
    void Fire<T>(T signal);
    void Unsubscribe<T>(Action<T> action);
}
```


### IEventBusAsync

```cs
public interface IEventBusAsync
{
    void Subscribe<T>(Func<T, UniTask> action);
    UniTask Fire<T>(T signal, CancellationToken token = default);
    void Unsubscribe<T>(Func<T, UniTask> action);
}
```

## Sync calls

For projects which use assembly definitions add to references `OsirisGames.EventBroker.Core`

### Usage Example

First create instance `EventBus` or your own implementation which implements `IEventBus` interface.

```cs
IEventBus _eventBus = new EventBus();
```

Subscribe to EventBus // add information that it's type related

## Async calls

For projects which use assembly definitions add to references `OsirisGames.EventBroker.Core`, `OsirisGames.EventBroker.Core.UniTask`
and also add to define symbols **`EVENTBROKER_UNITASK_ENABLED`**

### Usage Example

First create instance `EventBusAsync` or your own implementation which implements `IEventBusAsync` interface.

```cs
IEventBusAsync _eventBus = new EventBusAsync();
```