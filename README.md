# Sync

A reasonable and reliable way to run C# async tasks synchronously, without risking deadlocks.

## Use

```csharp
// Async method that returns 'Task':
Sync.Run(() => MyThing.DoSomethingAsync(...));

// Async method that returns 'Task<T>':
var result = Sync.Run(() => MyThing.ReadSomethingAsync(...));
```
