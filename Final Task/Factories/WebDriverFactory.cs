using FinalTask.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FinalTask.Factories;

public static class WebDriverFactory
{
    private static readonly Uri RemoteWebDriverUrl = new(TestsConfig.SeleniumGridUrl);

    /// <summary>
    /// Instantiates a new instance of <see cref="RemoteWebDriver"/> based on provided options.
    /// The driver is connected to Selenium Grid based on the URL specified in configuration.
    /// </summary>
    /// <param name="browserOptions"></param>
    /// <returns></returns>
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
