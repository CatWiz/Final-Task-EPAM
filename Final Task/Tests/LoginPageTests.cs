using FinalTask.Config;
using FinalTask.Factories;
using FinalTask.PageObjects;
using FluentAssertions;
using OpenQA.Selenium;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageTests : IDisposable
{
    private IWebDriver? _driver;

    public static IEnumerable<(WebDriverType, Func<DriverOptions>?)> TestCases
    {
        get
        {
            yield return (WebDriverType.Chrome, null);
            yield return (WebDriverType.Firefox, null);
            yield return (WebDriverType.Edge, null);
        }
    }

    private void InitializeDriver(WebDriverType type, Func<DriverOptions>? optionsFactory)
    {
        this._driver = WebDriverFactory.GetDriver(type, optionsFactory);
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
    public void LoginAfterClearingCredentialsShouldShowUsernameMissingError(WebDriverType type, Func<DriverOptions>? optionsFactory)
    {
        this.InitializeDriver(type, optionsFactory);

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
    public void LoginAfterClearingPasswordShouldShowPasswordMissingError(WebDriverType type, Func<DriverOptions>? optionsFactory)
    {
        this.InitializeDriver(type, optionsFactory);

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
    public void LoginWithValidCredentialsShouldRedirectToInventoryPage(WebDriverType type, Func<DriverOptions>? optionsFactory)
    {
        this.InitializeDriver(type, optionsFactory);

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