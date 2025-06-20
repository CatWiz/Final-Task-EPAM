using FinalTask.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FinalTask.Factories;

public static class WebDriverFactory
{
    private static readonly Uri RemoteWebDriverUrl = new(TestsConfig.SeleniumGridUrl);

    public static IWebDriver GetDriver(BrowserOptions browserOptions)
    {
        var capabilities = browserOptions.DriverOptions.ToCapabilities();
        var remoteDriver = new RemoteWebDriver(RemoteWebDriverUrl, capabilities);
        if (browserOptions.Maximize)
        {
            remoteDriver.Manage().Window.Maximize();
        }

        return remoteDriver;
    }
}
