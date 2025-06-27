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
    public override string ToString()
    {
        string windowSize;
        if (this.Context.WindowSize.HasValue)
        {
            var (width, height) = this.Context.WindowSize.Value;
            windowSize = $"{width}x{height}";
        }
        else if (this.Maximize)
        {
            windowSize = "Maximized";
        }
        else
        {
            windowSize = "Default Size";
        }

        return string.Join(" ", [
            $"{this.DriverOptions.BrowserName}",
            $"Window size: {windowSize}",
            $"Headless: {this.Context.Headless}",
            $"Incognito: {this.Context.Incognito}",
            $"Arguments: {string.Join(", ", this.Context.Arguments)}",
            $"Preferences: {string.Join(", ", this.Context.Preferences.Select(kvp => $"{kvp.Key}={kvp.Value}"))}",
            $"Additional Options: {string.Join(", ", this.Context.AdditionalOptions.Select(kvp => $"{kvp.Key}={kvp.Value}"))}"
        ]);
    }
}
