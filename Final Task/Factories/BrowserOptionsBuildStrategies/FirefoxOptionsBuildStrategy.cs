using OpenQA.Selenium.Firefox;

namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public class FirefoxOptionsBuildStrategy : IBrowserOptionsBuildStrategy
{
    public BrowserOptionsContext ConfigureArguments(BrowserOptionsContext context)
    {
        if (context.Headless)
        {
            context.Arguments.Add("-headless=new");
        }

        if (context.Incognito)
        {
            context.Arguments.Add("-private");
        }

        if (context.WindowSize is not null)
        {
            var (width, height) = context.WindowSize.Value;
            context.Arguments.Add($"-width={width}");
            context.Arguments.Add($"-height={height}");
        }

        return context;
    }
    public BrowserOptions BuildBrowserOptions(BrowserOptionsContext context)
    {
        var options = new FirefoxOptions();

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