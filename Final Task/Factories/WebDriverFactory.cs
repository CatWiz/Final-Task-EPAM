using FinalTask.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FinalTask.Factories;

public static class WebDriverFactory
{
    private static readonly Uri RemoteWebDriverUrl = new(TestsConfig.SeleniumGridUrl);

    public static IWebDriver GetDriver(Func<DriverOptions> optionsFactory, bool maximize)
    {
        var options = optionsFactory.Invoke();
        var remoteDriver = new RemoteWebDriver(RemoteWebDriverUrl, options.ToCapabilities());
        if (maximize)
        {
            remoteDriver.Manage().Window.Maximize();
        }

        return remoteDriver;
    }
}
