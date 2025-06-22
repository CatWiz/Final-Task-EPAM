using OpenQA.Selenium.Firefox;

namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public class FirefoxOptionsBuildStrategy : IBrowserOptionsBuildStrategy
{
    public BrowserOptions BuildBrowserOptions(BrowserOptionsContext context)
    {
        var options = new FirefoxOptions();

        if (context.Headless)
        {
            options.AddArgument("-headless=new");
        }

        if (context.Incognito)
        {
            options.AddArgument("-private");
        }

        if (context.WindowSize is not null)
        {
            var (width, height) = context.WindowSize.Value;
            options.AddArgument($"-width={width}");
            options.AddArgument($"-height={height}");
        }

        options.AddArguments(context.Arguments);

        foreach (var preference in context.Preferences)
        {
            options.SetPreference(preference.Key, preference.Value);
        }

        foreach (var additionalOption in context.AdditionalOptions)
        {
            options.AddAdditionalOption(additionalOption.Key, additionalOption.Value);
        }

        return new BrowserOptions
        {
            DriverOptions = options,
            Maximize = context.Maximize,
            Context = context
        };
    }
}