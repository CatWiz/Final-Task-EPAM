using FinalTask.Config;
using FinalTask.Factories;
using FinalTask.Logging;
using FinalTask.PageObjects;
using FluentAssertions;
using OpenQA.Selenium;
using Serilog;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageTests : IDisposable
{
    private readonly ILogger Logger = LoggerFactory.GetLogger();
    private IWebDriver? _driver;
    private readonly TestContext _testContext;

    public static IEnumerable<object[]> TestCases
    {
        get
        {
            yield return [BrowserOptionsRepository.ChromeHeadless];
            yield return [BrowserOptionsRepository.FirefoxHeadless];
            yield return [BrowserOptionsRepository.EdgeHeadless];

            yield return [BrowserOptionsRepository.ChromeHeadlessTiny];
            yield return [BrowserOptionsRepository.FirefoxHeadlessTiny];
            yield return [BrowserOptionsRepository.EdgeHeadlessTiny];
        }
    }

    private void InitializeDriver(BrowserOptions options)
    {
        this.Logger.Information("Initializing driver for {TestName} for {Browser} browser",
            this._testContext.TestName,
            options.DriverOptions.BrowserName);
        this.Logger.Information("Browser capabilities: {Capabilities}",
            options.DriverOptions.ToCapabilities());

        this._driver = WebDriverFactory.GetDriver(options);
        this._driver.Navigate().GoToUrl(TestsConfig.BaseUrl);
    }

    public LoginPageTests(TestContext testContext)
    {
        this._testContext = testContext;
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
    public void LoginAfterClearingCredentialsShouldShowUsernameMissingError(BrowserOptions options)
    {
        this.InitializeDriver(options);

        var loginPage = new LoginPageObject(this._driver ?? throw new InvalidOperationException("Driver not initialized"));
        var username = "asdf";
        var password = "zxcv";

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Information("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Information("Entered password {password}, current value: {CurrentPassword}",
            password, loginPage.GetPassword());

        loginPage.ClearUsername();
        loginPage.ClearPassword();

        this.Logger.Information("Cleared username, current value: {CurrentUsername}", loginPage.GetUsername());
        this.Logger.Information("Cleared password, current value: {CurrentPassword}", loginPage.GetPassword());

        loginPage.LoginExpectFailure();

        this.Logger.Information("Tried logging in with cleared credentials, current URL: {CurrentUrl}",
            this._driver.Url);

        _ = this._driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        this.Logger.Information("Verifying error message after login attempt, error message: {ErrorMessage}",
            errorMessage);

        _ = errorMessage
            .Should().Be("Epic sadface: Username is required", $"error message should indicate missing username, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(TestCases), DynamicDataSourceType.Property)]
    public void LoginAfterClearingPasswordShouldShowPasswordMissingError(BrowserOptions options)
    {
        this.InitializeDriver(options);

        var loginPage = new LoginPageObject(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        var username = "asdf";
        var password = "zxcv";

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Information("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Information("Entered password {password}, current value: {CurrentPassword}",
            password, loginPage.GetPassword());

        loginPage.ClearPassword();

        this.Logger.Information("Cleared password, current value: {CurrentPassword}", loginPage.GetPassword());

        loginPage.LoginExpectFailure();

        this.Logger.Information("Tried logging in with cleared password, current URL: {CurrentUrl}",
            this._driver.Url);

        _ = this._driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        this.Logger.Information("Verifying error message after login attempt, error message: {ErrorMessage}",
            errorMessage);

        _ = errorMessage
            .Should().Be("Epic sadface: Password is required", $"error message should indicate missing password, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(TestCases), DynamicDataSourceType.Property)]
    public void LoginWithValidCredentialsShouldRedirectToInventoryPage(BrowserOptions options)
    {
        this.InitializeDriver(options);

        var loginPage = new LoginPageObject(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        var username = "standard_user";
        var password = "secret_sauce";

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Information("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Information("Entered password {password}, current value: {CurrentPassword}",
            password, loginPage.GetPassword());

        _ = loginPage.LoginExpectSuccess();

        this.Logger.Information("Login successful, current URL: {CurrentUrl}", this._driver.Url);

        _ = this._driver.Url
            .Should().Be($"{TestsConfig.BaseUrl}/inventory.html", "login with valid credentials should redirect to the inventory page");
    }

    public void Dispose()
    {
        this._driver?.Quit();
        this._driver?.Dispose();
    }
}