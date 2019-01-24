// ReSharper disable once CheckNamespace
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
                TaskScheduler.Default);

        /// <summary>
        /// Run an async function synchronously and return the result
        /// </summary>
        public static TResult Run<TResult>(Func<Task<TResult>> func)
        {
            if (_taskFactory == null) throw new Exception("Static init failed");
            if (func == null) throw new ArgumentNullException(nameof(func));

            var rawTask = _taskFactory.StartNew(func).Unwrap();
            if (rawTask == null) throw new Exception("Invalid task");

            return rawTask.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Run an async function synchronously
        /// </summary>
        public static void Run(Func<Task> func)
        {
            if (_taskFactory == null) throw new Exception("Static init failed");
            if (func == null) throw new ArgumentNullException(nameof(func));

            var rawTask = _taskFactory.StartNew(func).Unwrap();
            if (rawTask == null) throw new Exception("Invalid task");

            rawTask.GetAwaiter().GetResult();
        }
    }
}