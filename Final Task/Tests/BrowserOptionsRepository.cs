using FinalTask.Factories;
using BrowserType = FinalTask.Factories.BrowserOptionsBuilder.BrowserType;

namespace FinalTask.Tests;

public static class BrowserOptionsProvider
{
    public static BrowserOptions ChromeHeadless => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Chrome)
            .WithHeadless()
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

    public static BrowserOptions EdgeHeadless => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Edge)
            .WithHeadless()
            .Build(),
        Maximize = true
    };

    public static BrowserOptions ChromeHeadlessTiny => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Chrome)
            .WithHeadless()
            .AddArgument("--window-size=800,600")
            .Build(),
        Maximize = false
    };

    public static BrowserOptions FirefoxHeadlessTiny => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Firefox)
            .WithHeadless()
            .AddArgument("--window-size=800,600")
            .Build(),
        Maximize = false
    };

    public static BrowserOptions EdgeHeadlessTiny => new()
    {
        DriverOptions = BrowserOptionsBuilder.Create()
            .ForBrowser(BrowserType.Edge)
            .WithHeadless()
            .AddArgument("--window-size=800,600")
            .Build(),
        Maximize = false
    };
}