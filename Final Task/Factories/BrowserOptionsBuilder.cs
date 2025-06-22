using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace FinalTask.Factories;

public class BrowserOptionsBuilder
{
    private readonly List<string> _arguments = [];
    private readonly Dictionary<string, bool> _preferences = [];
    private readonly Dictionary<string, object> _additionalOptions = [];
    private BrowserType _browserType = BrowserType.Chrome;
    private bool _headless = false;
    private bool _maximize = false;

    public enum BrowserType
    {
        Chrome,
        Firefox,
        Edge
    }

    public BrowserOptionsBuilder ForBrowser(BrowserType browserType)
    {
        this._browserType = browserType;
        return this;
    }

    public BrowserOptionsBuilder WithHeadless(bool headless = true)
    {
        this._headless = headless;
        return this;
    }

    public BrowserOptionsBuilder AddArgument(string argument)
    {
        this._arguments.Add(argument);
        return this;
    }

    public BrowserOptionsBuilder AddArguments(params string[] arguments)
    {
        this._arguments.AddRange(arguments);
        return this;
    }

    public BrowserOptionsBuilder AddPreference(string key, bool value)
    {
        this._preferences[key] = value;
        return this;
    }

    public BrowserOptionsBuilder AddAdditionalOption(string key, object value)
    {
        this._additionalOptions[key] = value;
        return this;
    }

    public BrowserOptionsBuilder WithWindowSize(int width, int height)
    {
        return this.AddArgument($"--window-size={width},{height}");
    }

    public BrowserOptionsBuilder WithDisableWebSecurity()
    {
        return this.AddArgument("--disable-web-security");
    }

    public BrowserOptionsBuilder WithDisableGpu()
    {
        return this.AddArgument("--disable-gpu");
    }

    public BrowserOptionsBuilder WithNoSandbox()
    {
        return this.AddArgument("--no-sandbox");
    }

    public BrowserOptionsBuilder WithIncognito()
    {
        return this._browserType switch
        {
            BrowserType.Chrome => this.AddArgument("--incognito"),
            BrowserType.Edge => this.AddArgument("--inprivate"),
            BrowserType.Firefox => this.AddArgument("--private-window"),
            _ => this
        };
    }

    public BrowserOptionsBuilder WithMaximizeWindow()
    {
        this._maximize = true;
        return this;
    }

    public BrowserOptions Build()
    {
        var (args, preferences, additionalOptions) = this.GetOptionsAsString();
        return new BrowserOptions
        {
            DriverOptions = this._browserType switch
            {
                BrowserType.Chrome => this.BuildChromeOptions(),
                BrowserType.Firefox => this.BuildFirefoxOptions(),
                BrowserType.Edge => this.BuildEdgeOptions(),
                _ => throw new ArgumentException($"Unsupported browser type: {this._browserType}")
            },
            Maximize = this._maximize,
            Arguments = args,
            Preferences = preferences,
            AdditionalOptions = additionalOptions
        };
    }

    private (string args, string preferences, string additionalOptions) GetOptionsAsString()
    {
        var args = string.Join(" ", this._arguments);
        var preferences = string.Join(", ", this._preferences.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var additionalOptions = string.Join(", ", this._additionalOptions.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        return (args, preferences, additionalOptions);
    }

    private ChromeOptions BuildChromeOptions()
    {
        var options = new ChromeOptions();

        if (this._headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArguments(this._arguments);

        foreach (var preference in this._preferences)
        {
            options.AddUserProfilePreference(preference.Key, preference.Value);
        }

        foreach (var additionalOption in this._additionalOptions)
        {
            options.AddAdditionalOption(additionalOption.Key, additionalOption.Value);
        }

        return options;
    }

    private FirefoxOptions BuildFirefoxOptions()
    {
        var options = new FirefoxOptions();

        if (this._headless)
        {
            options.AddArgument("--headless");
        }

        options.AddArguments(this._arguments);

        foreach (var additionalOption in this._additionalOptions)
        {
            options.AddAdditionalOption(additionalOption.Key, additionalOption.Value);
        }

        // Firefox preferences are set differently
        foreach (var preference in this._preferences)
        {
            options.SetPreference(preference.Key, preference.Value);
        }

        return options;
    }

    private EdgeOptions BuildEdgeOptions()
    {
        var options = new EdgeOptions();

        if (this._headless)
        {
            options.AddArgument("--headless=new");
        }

        options.AddArguments(this._arguments);

        foreach (var preference in this._preferences)
        {
            options.AddUserProfilePreference(preference.Key, preference.Value);
        }

        foreach (var additionalOption in this._additionalOptions)
        {
            options.AddAdditionalOption(additionalOption.Key, additionalOption.Value);
        }

        return options;
    }

    public static BrowserOptionsBuilder Create() => new();
}
