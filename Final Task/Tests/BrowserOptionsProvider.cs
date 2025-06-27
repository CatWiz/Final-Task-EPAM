using FinalTask.Factories;
using FinalTask.Factories.BrowserOptionsBuildStrategies;

namespace FinalTask.Tests;

public static class BrowserOptionsProvider
{
    public static BrowserOptions ChromeHeadless => BrowserOptionsBuilder.Create()
        .WithBuildStrategy(new ChromeOptionsBuildStrategy())
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions FirefoxHeadless => BrowserOptionsBuilder.Create()
        .WithBuildStrategy(new FirefoxOptionsBuildStrategy())
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions EdgeHeadless => BrowserOptionsBuilder.Create()
        .WithBuildStrategy(new EdgeOptionsBuildStrategy())
        .WithHeadless()
        .WithMaximizeWindow()
        .Build();

    public static BrowserOptions ChromeHeadlessTiny => BrowserOptionsBuilder.Create()
        .WithBuildStrategy(new ChromeOptionsBuildStrategy())
        .WithHeadless()
        .WithWindowSize(800, 600)
        .Build();

    public static BrowserOptions FirefoxHeadlessTiny => BrowserOptionsBuilder.Create()
        .WithBuildStrategy(new FirefoxOptionsBuildStrategy())
        .WithHeadless()
        .WithWindowSize(800, 600)
        .Build();

    public static BrowserOptions EdgeHeadlessTiny => BrowserOptionsBuilder.Create()
        .WithBuildStrategy(new EdgeOptionsBuildStrategy())
        .WithHeadless()
        .WithWindowSize(800, 600)
        .Build();
}
