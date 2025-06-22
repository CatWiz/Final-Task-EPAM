using OpenQA.Selenium.Chrome;

namespace FinalTask.Factories.BrowserOptionsBuildStrategies;

public class ChromeOptionsBuildStrategy : IBrowserOptionsBuildStrategy
{
    public BrowserOptions BuildBrowserOptions(BrowserOptionsContext context)
    {
        var options = new ChromeOptions();

        if (context.Headless)
        {
            options.AddArgument("--headless=new");
        }

        if (context.Incognito)
        {
            options.AddArgument("--incognito");
        }

        if (context.WindowSize is not null)
        {
            var (width, height) = context.WindowSize.Value;
            options.AddArgument($"--window-size={width},{height}");
        }

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