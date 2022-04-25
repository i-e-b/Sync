// ReSharper disable once CheckNamespace

using System;
using JetBrains.Annotations;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Helper class to properly wait for async tasks
    /// </summary>
    public static class Sync  
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default ?? TaskScheduler.Current ?? TaskScheduler.FromCurrentSynchronizationContext()!);

        /// <summary>
        /// Run an async function synchronously and return the result
        /// </summary>
        public static TResult Run<TResult>([InstantHandle]Func<Task<TResult>> func)
        {
            if (_taskFactory == null) throw new Exception("Static init failed");
            if (func == null) throw new ArgumentNullException(nameof(func));

            var rawTask = _taskFactory.StartNew(func)?.Unwrap();
            if (rawTask == null) throw new Exception("Invalid task");

            return rawTask.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Run an async function synchronously
        /// </summary>
        public static void Run([InstantHandle]Func<Task> func)
        {
            if (_taskFactory == null) throw new Exception("Static init failed");
            if (func == null) throw new ArgumentNullException(nameof(func));

            var rawTask = _taskFactory.StartNew(func)?.Unwrap();
            if (rawTask == null) throw new Exception("Invalid task");

            rawTask.GetAwaiter().GetResult();
        }
    }
}


namespace JetBrains.Annotations
{
    /// <summary>
    /// Tells the code analysis engine if the parameter is completely handled when the invoked method is on stack.
    /// If the parameter is a delegate, indicates that delegate is executed while the method is executed.
    /// If the parameter is an enumerable, indicates that it is enumerated while the method is executed.
    /// </summary>
    internal class InstantHandleAttribute : Attribute { }
}