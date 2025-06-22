using OpenQA.Selenium;

namespace FinalTask.Factories;

public class BrowserOptions
{
    public required DriverOptions DriverOptions { get; init; }
    public required bool Maximize { get; init; }
    public required string Arguments { get; init; }
    public required string Preferences { get; init; }
    public required string Capabilities { get; init; }
}
