using FinalTask.Factories;
using Serilog;

namespace FinalTask.Extensions;

public static class LoggerExtensions
{
    /// <summary>
    /// Logs browser options, capabilities, and command line arguments, as well as any additional options.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public static void LogBrowserOptions(this ILogger logger, BrowserOptions options)
    {
        logger.Information("Browser capabilities: {Capabilities}", options.DriverOptions.ToCapabilities());

        var context = options.Context;
        if (context.Maximize)
        {
            logger.Information("Browser window will be maximized.");
        }
        else
        {
            logger.Information("Browser window will not be maximized.");
        }

        if (context.Arguments.Count > 0)
        {
            logger.Information("Browser arguments: {Arguments}", string.Join(" ", context.Arguments));
        }

        if (context.Preferences.Count > 0)
        {
            logger.Information("Browser preferences: {Preferences}",
                string.Join(" ", context.Preferences.Select(kvp => $"{kvp.Key}={kvp.Value}")));
        }

        if (context.AdditionalOptions.Count > 0)
        {
            logger.Information("Browser additional options: {AdditionalOptions}",
                string.Join(" ", context.AdditionalOptions.Select(kvp => $"{kvp.Key}={kvp.Value}")));
        }
    }
}
