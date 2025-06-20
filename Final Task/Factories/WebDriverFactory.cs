using FinalTask.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace FinalTask.Factories;

public enum WebDriverType
{
    Chrome,
    Firefox
}

public static class WebDriverFactory
{
    private static readonly ThreadLocal<IWebDriver> LocalDriver = new();
    private static readonly Uri RemoteWebDriverUrl = new(TestsConfig.SeleniumGridUrl);

    public static IWebDriver GetDriver(WebDriverType type, Func<DriverOptions>? optionsFactory = null)
    {
        if (LocalDriver.Value is not null)
        {
            return LocalDriver.Value;
        }

        DriverOptions options = type switch
        {
            WebDriverType.Chrome => new ChromeOptions(),
            WebDriverType.Firefox => new FirefoxOptions(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        if (optionsFactory is not null)
        {
            options = optionsFactory.Invoke();
        }

        var remoteDriver = new RemoteWebDriver(RemoteWebDriverUrl, options.ToCapabilities());
        remoteDriver.Manage().Window.Maximize();

        LocalDriver.Value = remoteDriver;
        return LocalDriver.Value;
    }
}
