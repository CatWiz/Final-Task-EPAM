using FinalTask.Config;
using Serilog;
using Serilog.Events;

namespace FinalTask.Logging;

public static class LoggerFactory
{
    private static readonly ThreadLocal<ILogger> _localLogger = new();
    private static readonly DateTime Now = DateTime.Now;
    private static int ThreadIndex = 0;
    private static readonly Dictionary<int, int> ThreadIdToIndex = [];

    /// <summary>
    /// Gets the incremental index of the current thread based on its ID.
    /// </summary>
    /// <returns>Thread index</returns>
    private static int GetCurrentThreadIndex()
    {
        int threadId = Environment.CurrentManagedThreadId;
        if (!ThreadIdToIndex.TryGetValue(threadId, out int index))
        {
            index = ThreadIndex++;
            ThreadIdToIndex[threadId] = index;
        }
        return index;
    }

    /// <summary>
    /// Creates or retrieves a logger instance for the current thread.
    /// The logger is configured based on settings in appsettings.json file.
    /// The log file path includes thread index to avoid conflicts when running tests in parallel.
    /// </summary>
    /// <returns>Logger instance</returns>
    public static ILogger GetLogger()
    {
        if (_localLogger.Value is not null)
        {
            return _localLogger.Value;
        }

        string path = TestsConfig.LogOutputPath
            .Replace("{Date}", Now.ToString("yyyyMMdd"))
            .Replace("{Time}", Now.ToString("HHmmss"))
            .Replace("{ThreadIdx}", GetCurrentThreadIndex().ToString());

        var logLevel = Enum.Parse<LogEventLevel>(TestsConfig.LogOutputLevel, true);

        var logger = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .WriteTo.File(path, rollingInterval: RollingInterval.Infinite)
            .CreateLogger();

        _localLogger.Value = logger;
        return _localLogger.Value;
    }
}