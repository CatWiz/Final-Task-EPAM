using OpenQA.Selenium.Chrome;

namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public class ChromeOptionsBuildStrategy : IBrowserOptionsBuildStrategy
{
    public BrowserOptionsContext ConfigureArguments(BrowserOptionsContext context)
    {
        if (context.Headless)
        {
            context.Arguments.Add("--headless=new");
        }

        if (context.Incognito)
        {
            context.Arguments.Add("--incognito");
        }

        if (context.WindowSize is not null)
        {
            var (width, height) = context.WindowSize.Value;
            context.Arguments.Add($"--window-size={width},{height}");
        }

        return context;
    }
    public BrowserOptions BuildBrowserOptions(BrowserOptionsContext context)
    {
        var options = new ChromeOptions();

        options.AddArguments(context.Arguments);

        foreach (var preference in context.Preferences)
        {
            options.AddUserProfilePreference(preference.Key, preference.Value);
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