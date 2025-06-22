namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public interface IBrowserOptionsBuildStrategy
{
    BrowserOptionsContext ConfigureArguments(BrowserOptionsContext context);
    BrowserOptions BuildBrowserOptions(BrowserOptionsContext context);
}