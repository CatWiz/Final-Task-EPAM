using FinalTask.Factories;
using BrowserType = FinalTask.Factories.BrowserOptionsBuilder.BrowserType;

namespace FinalTask.Tests;

public static class BrowserOptionsProvider
{
    public static BrowserOptions ChromeHeadless => BrowserOptionsBuilder.Create()
        .ForBrowser(BrowserType.Chrome)
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions FirefoxHeadless => BrowserOptionsBuilder.Create()
        .ForBrowser(BrowserType.Firefox)
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions EdgeHeadless => BrowserOptionsBuilder.Create()
        .ForBrowser(BrowserType.Edge)
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions ChromeHeadlessTiny => BrowserOptionsBuilder.Create()
        .ForBrowser(BrowserType.Chrome)
        .WithHeadless()
        .AddArgument("--window-size=800,600")
        .Build();

    public static BrowserOptions FirefoxHeadlessTiny => BrowserOptionsBuilder.Create()
        .ForBrowser(BrowserType.Firefox)
        .WithHeadless()
        .AddArgument("--window-size=800,600")
        .Build();

    public static BrowserOptions EdgeHeadlessTiny => BrowserOptionsBuilder.Create()
        .ForBrowser(BrowserType.Edge)
        .WithHeadless()
        .AddArgument("--window-size=800,600")
        .Build();
}