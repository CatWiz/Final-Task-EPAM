using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace FinalTask.Factories;

public class BrowserOptionsBuilder
{
    private readonly List<string> _arguments = [];
    private readonly Dictionary<string, bool> _preferences = [];
    private readonly Dictionary<string, object> _capabilities = [];
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

    public BrowserOptionsBuilder AddCapability(string key, object value)
    {
        this._capabilities[key] = value;
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
        var (args, preferences, capabilities) = this.GetOptionsAsString();
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
            Capabilities = capabilities
        };
    }

    private (string args, string preferences, string capabilities) GetOptionsAsString()
    {
        var args = string.Join(" ", this._arguments);
        var preferences = string.Join(", ", this._preferences.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var capabilities = string.Join(", ", this._capabilities.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        return (args, preferences, capabilities);
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

        foreach (var capability in this._capabilities)
        {
            options.AddAdditionalOption(capability.Key, capability.Value);
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

        foreach (var capability in this._capabilities)
        {
            options.AddAdditionalOption(capability.Key, capability.Value);
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

        foreach (var capability in this._capabilities)
        {
            options.AddAdditionalOption(capability.Key, capability.Value);
        }

        return options;
    }

    public static BrowserOptionsBuilder Create() => new();
}
