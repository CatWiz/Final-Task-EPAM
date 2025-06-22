using FinalTask.Factories;
using Serilog;

namespace FinalTask.Extensions;

public static class ILoggerExtensions
{
    public static void LogBrowserOptions(this ILogger logger, BrowserOptions options)
    {
        logger.Verbose("Browser capabilities: {Capabilities}", options.DriverOptions.ToCapabilities());

        if (!string.IsNullOrEmpty(options.Arguments))
        {
            logger.Verbose("Browser arguments: {Arguments}", options.Arguments);
        }

        if (!string.IsNullOrEmpty(options.Preferences))
        {
            logger.Verbose("Browser preferences: {Preferences}", options.Preferences);
        }

        if (!string.IsNullOrEmpty(options.AdditionalOptions))
        {
            logger.Verbose("Browser additional options: {AdditionalOptions}", options.AdditionalOptions);
        }
    }
}