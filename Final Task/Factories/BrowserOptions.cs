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
    /// Context object from which the browser options were built.
    /// Used for logging purposes.
    /// </summary>
    public required BrowserOptionsContext Context { get; init; }

    public string? DisplayName { get; init; }

    public override string ToString()
    {
        return $"BrowserOptions: {this.DriverOptions.ToCapabilities()}, " +
               $"Maximize: {this.Maximize}, " +
               $"Headless: {this.Context.Headless}, " +
               $"Incognito: {this.Context.Incognito}, ";
    }
}
