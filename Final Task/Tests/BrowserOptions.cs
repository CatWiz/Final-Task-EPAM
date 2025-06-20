using System.Diagnostics;
using FinalTask.Factories;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

using BrowserType = FinalTask.Factories.BrowserOptionsBuilder.BrowserType;

namespace FinalTask.Tests;

[DebuggerDisplay("{BrowserType}")]
public class BrowserOptions
{
    public BrowserType BrowserType
    {
        get
        {
            return this.DriverOptions switch
            {
                ChromeOptions => BrowserType.Chrome,
                FirefoxOptions => BrowserType.Firefox,
                EdgeOptions => BrowserType.Edge,
                _ => throw new NotSupportedException("Unsupported browser type")
            };
        }
    }
    public required DriverOptions DriverOptions { get; init; }
    public bool Maximize { get; init; }
}

public static class BrowserOptionsRepository
{
    public static BrowserOptions ChromeDefault => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Chrome)
            .Build(),
        Maximize = true
    };

    public static BrowserOptions ChromeHeadless => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Chrome)
            .WithHeadless()
            .Build(),
        Maximize = true
    };

    public static BrowserOptions FirefoxDefault => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Firefox)
            .Build(),
        Maximize = true
    };

    public static BrowserOptions FirefoxHeadless => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Firefox)
            .WithHeadless()
            .Build(),
        Maximize = true
    };

    public static BrowserOptions EdgeDefault => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Edge)
            .Build(),
        Maximize = true
    };

    public static BrowserOptions EdgeHeadless => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Edge)
            .WithHeadless()
            .Build(),
        Maximize = true
    };
}