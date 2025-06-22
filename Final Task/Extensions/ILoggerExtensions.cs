using FinalTask.Factories;
using Serilog;

namespace FinalTask.Extensions;

public static class ILoggerExtensions
{
    public static void LogBrowserOptions(this ILogger logger, BrowserOptions options)
    {
        logger.Verbose("Browser capabilities: {Capabilities}", options.DriverOptions.ToCapabilities());

        var context = options.Context;
        if (context is null)
        {
            logger.Verbose("Browser context is not set.");
            return;
        }

        if (context.Incognito)
        {
            logger.Verbose("Browser is running in incognito mode.");
        }
        else
        {
            logger.Verbose("Browser is not running in incognito mode.");
        }

        if (context.WindowSize is not null)
        {
            var (width, height) = context.WindowSize.Value;
            logger.Verbose("Browser window size: {Width}x{Height}", width, height);
        }

        if (context.Arguments.Count > 0)
        {
            logger.Verbose("Browser arguments:\n\t{Arguments}", string.Join("\n\t", context.Arguments));
        }

        if (context.Preferences.Count > 0)
        {
            logger.Verbose("Browser preferences:\n\t{Preferences}",
                string.Join("\n\t", context.Preferences.Select(kvp => $"{kvp.Key}={kvp.Value}")));
        }

        if (context.AdditionalOptions.Count > 0)
        {
            logger.Verbose("Browser additional options:\n\t{AdditionalOptions}",
                string.Join("\n\t", context.AdditionalOptions.Select(kvp => $"{kvp.Key}={kvp.Value}")));
        }
    }
}