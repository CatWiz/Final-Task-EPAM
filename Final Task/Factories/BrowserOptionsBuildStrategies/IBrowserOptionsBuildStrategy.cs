namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public interface IBrowserOptionsBuildStrategy
{
    BrowserOptions BuildBrowserOptions(BrowserOptionsContext context);
}