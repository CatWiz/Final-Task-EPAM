namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public interface IBrowserOptionsBuildStrategy
{
    /// <summary>
    /// Converts high-level browser options (such as window size, headless mode)
    /// into specific arguments for the browser driver.
    /// This method should be called right before building the browser options.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    BrowserOptionsContext ConfigureArguments(BrowserOptionsContext context);

    /// <summary>
    /// Generates a BrowserOptions object based on the provided context.
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Browser options</returns>
    BrowserOptions BuildBrowserOptions(BrowserOptionsContext context);
}