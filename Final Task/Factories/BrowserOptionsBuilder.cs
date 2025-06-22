using FinalTask.Factories.BrowserOptionsBuildStrategies;

namespace FinalTask.Factories;

public class BrowserOptionsBuilder
{
    private readonly BrowserOptionsContext _context = new();
    private IBrowserOptionsBuildStrategy? _strategy;

    public enum BrowserType
    {
        Chrome,
        Firefox,
        Edge
    }

    public BrowserOptionsBuilder WithBuildStrategy(IBrowserOptionsBuildStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(strategy);
        this._strategy = strategy;
        return this;
    }

    public BrowserOptionsBuilder WithHeadless(bool headless = true)
    {
        this._context.Headless = headless;
        return this;
    }

    public BrowserOptionsBuilder AddArgument(string argument)
    {
        this._context.Arguments.Add(argument);
        return this;
    }

    public BrowserOptionsBuilder AddArguments(params string[] arguments)
    {
        if (arguments == null || arguments.Length == 0)
        {
            throw new ArgumentException("At least one argument must be provided.", nameof(arguments));
        }

        this._context.Arguments.AddRange(arguments);
        return this;
    }

    public BrowserOptionsBuilder AddPreference(string key, bool value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Preference key cannot be null or empty.", nameof(key));
        }

        this._context.Preferences.Add(key, value);
        return this;
    }

    public BrowserOptionsBuilder AddAdditionalOption(string key, object value)
    {
        this._context.AdditionalOptions.Add(key, value);
        return this;
    }

    public BrowserOptionsBuilder WithWindowSize(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(height, 0);

        this._context.WindowSize = (width, height);
        return this;
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
        this._context.Incognito = true;
        return this;
    }

    public BrowserOptionsBuilder WithMaximizeWindow()
    {
        this._context.Maximize = true;
        return this;
    }

    public BrowserOptions Build()
    {
        if (this._strategy == null)
        {
            throw new InvalidOperationException("Browser strategy must be set before building options.");
        }

        return this._strategy.BuildBrowserOptions(this._context);
    }

    public static BrowserOptionsBuilder Create() => new();
}
