using FinalTask.Factories;
using FinalTask.Factories.BrowserOptionsBuildStrategies;

namespace FinalTask.Tests;

public static class BrowserOptionsProvider
{
    public static BrowserOptions ChromeHeadless => BrowserOptionsBuilder.Create()
        .WithDisplayName("ChromeHeadless")
        .WithBuildStrategy(new ChromeOptionsBuildStrategy())
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions FirefoxHeadless => BrowserOptionsBuilder.Create()
        .WithDisplayName("FirefoxHeadless")
        .WithBuildStrategy(new FirefoxOptionsBuildStrategy())
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions EdgeHeadless => BrowserOptionsBuilder.Create()
        .WithDisplayName("EdgeHeadless")
        .WithBuildStrategy(new EdgeOptionsBuildStrategy())
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions ChromeHeadlessTiny => BrowserOptionsBuilder.Create()
        .WithDisplayName("ChromeHeadlessTiny")
        .WithBuildStrategy(new ChromeOptionsBuildStrategy())
        .WithHeadless()
        .WithWindowSize(800, 600)
        .Build();

    public static BrowserOptions FirefoxHeadlessTiny => BrowserOptionsBuilder.Create()
        .WithDisplayName("FirefoxHeadlessTiny")
        .WithBuildStrategy(new FirefoxOptionsBuildStrategy())
        .WithHeadless()
        .WithWindowSize(800, 600)
        .Build();

    public static BrowserOptions EdgeHeadlessTiny => BrowserOptionsBuilder.Create()
        .WithDisplayName("EdgeHeadlessTiny")
        .WithBuildStrategy(new EdgeOptionsBuildStrategy())
        .WithHeadless()
        .WithWindowSize(800, 600)
        .Build();
}
