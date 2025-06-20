using FinalTask.Config;
using FinalTask.Factories;
using FinalTask.PageObjects;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageTests : IDisposable
{
    private IWebDriver? _driver;

    public static IEnumerable<(Func<DriverOptions>, bool)> TestCases
    {
        get
        {
            yield return (() => new ChromeOptions(), true);
            yield return (() => new ChromeOptions(), false);
            yield return (() =>
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless=new");
                return options;
            }, true);

            yield return (() => new FirefoxOptions(), true);
            yield return (() => new FirefoxOptions(), false);

            yield return (() => new EdgeOptions(), true);
            yield return (() => new EdgeOptions(), false);
        }
    }

    private void InitializeDriver(Func<DriverOptions> options, bool maximize)
    {
        this._driver = WebDriverFactory.GetDriver(options, maximize);
        this._driver.Navigate().GoToUrl(TestsConfig.BaseUrl);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        this._driver?.Quit();
        this._driver?.Dispose();
        this._driver = null;
    }

    [TestMethod]
    [DynamicData(nameof(TestCases), DynamicDataSourceType.Property)]
    public void LoginAfterClearingCredentialsShouldShowUsernameMissingError(Func<DriverOptions> options, bool maximize)
    {
        this.InitializeDriver(options, maximize);

        var loginPage = new LoginPageObject(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        loginPage.EnterUsername("asdf");
        loginPage.EnterPassword("zxcv");

        loginPage.ClearUsername();
        loginPage.ClearPassword();

        loginPage.LoginExpectFailure();

        _ = this._driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        _ = errorMessage
            .Should().Be("Epic sadface: Username is required", $"error message should indicate missing username, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(TestCases), DynamicDataSourceType.Property)]
    public void LoginAfterClearingPasswordShouldShowPasswordMissingError(Func<DriverOptions> options, bool maximize)
    {
        this.InitializeDriver(options, maximize);

        var loginPage = new LoginPageObject(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        loginPage.EnterUsername("asdf");
        loginPage.EnterPassword("zxcv");

        loginPage.ClearPassword();

        loginPage.LoginExpectFailure();

        _ = this._driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        _ = errorMessage
            .Should().Be("Epic sadface: Password is required", $"error message should indicate missing password, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(TestCases), DynamicDataSourceType.Property)]
    public void LoginWithValidCredentialsShouldRedirectToInventoryPage(Func<DriverOptions> options, bool maximize)
    {
        this.InitializeDriver(options, maximize);

        var loginPage = new LoginPageObject(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        loginPage.EnterUsername("standard_user");
        loginPage.EnterPassword("secret_sauce");

        _ = loginPage.LoginExpectSuccess();

        _ = this._driver.Url
            .Should().Be($"{TestsConfig.BaseUrl}/inventory.html", "login with valid credentials should redirect to the inventory page");
    }

    public void Dispose()
    {
        this._driver?.Quit();
        this._driver?.Dispose();
    }
}