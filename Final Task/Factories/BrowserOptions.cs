using OpenQA.Selenium;

namespace FinalTask.Factories;

public class BrowserOptions
{
    public required DriverOptions DriverOptions { get; init; }

    /// <summary>
    /// Indicates whether the browser window should be maximized.
    /// </summary>
    public required bool Maximize { get; init; }

    /// <summary>
    /// String representation of command line arguments for the browser.
    /// Used for logging.
    /// </summary>
    public required string Arguments { get; init; }

    /// <summary>
    /// String representation of browser preferences.
    /// Used for logging.
    /// </summary>
    public required string Preferences { get; init; }

    /// <summary>
    /// String representation of additional browser options.
    /// Used for logging.
    /// </summary>
    public required string AdditionalOptions { get; init; }
}
