using System.Collections.Concurrent;
using FinalTask.Config;
using Serilog;
using Serilog.Events;

namespace FinalTask.Factories;

public static class LoggerFactory
{
    private static readonly ThreadLocal<ILogger> _threadLocalLogger = new();

    // Used for specifying date and time in log file names
    // Should be static to ensure consistent date/time is used across all threads
    private static readonly DateTime _initTime = DateTime.Now;
    private static int _nextThreadIndex = 0;
    private static readonly ConcurrentDictionary<int, int> _threadIdToIndex = [];

    /// <summary>
    /// Gets the incremental index of the current thread based on its ID.
    /// </summary>
    /// <returns>Thread index</returns>
    private static int GetCurrentThreadIndex()
    {
        var threadId = Environment.CurrentManagedThreadId;
        if (!_threadIdToIndex.TryGetValue(threadId, out var index))
        {
            index = Interlocked.Increment(ref _nextThreadIndex);
            _threadIdToIndex[threadId] = index;
        }
        return index;
    }

    /// <summary>
    /// Creates or retrieves a logger instance specific to the current thread.
    /// The logger is configured based on settings in appsettings.json file.
    /// The log file path includes thread index to avoid conflicts when running tests in parallel.
    /// </summary>
    /// <returns>Logger instance</returns>
    public static ILogger GetLogger()
    {
        if (_threadLocalLogger.Value is not null)
        {
            return _threadLocalLogger.Value;
        }

        var path = TestsConfig.LogOutputPath
            .Replace("{Date}", _initTime.ToString("yyyyMMdd"))
            .Replace("{Time}", _initTime.ToString("HHmmss"))
            .Replace("{ThreadIdx}", GetCurrentThreadIndex().ToString());

        var logLevel = Enum.Parse<LogEventLevel>(TestsConfig.LogOutputLevel, true);

        var logger = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .WriteTo.File(path, rollingInterval: RollingInterval.Infinite)
            .CreateLogger();

        _threadLocalLogger.Value = logger;
        return _threadLocalLogger.Value;
    }
}
