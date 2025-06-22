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

    public static IEnumerable<object[]> BrowserOptions
    {
        get
        {
            yield return [BrowserOptionsProvider.ChromeHeadless];
            yield return [BrowserOptionsProvider.FirefoxHeadless];
            yield return [BrowserOptionsProvider.EdgeHeadless];

            yield return [BrowserOptionsProvider.ChromeHeadlessTiny];
            yield return [BrowserOptionsProvider.FirefoxHeadlessTiny];
            yield return [BrowserOptionsProvider.EdgeHeadlessTiny];
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
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithoutUsername_ShowsUsernameRequiredError(BrowserOptions options)
    {
        this.InitializeDriver(options);

        var loginPage = new LoginPage(this._driver ?? throw new InvalidOperationException("Driver not initialized"));
        var username = "asdf";
        var password = "zxcv";

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
            password, loginPage.GetPassword());

        loginPage.ClearUsername();
        loginPage.ClearPassword();

        this.Logger.Verbose("Cleared username, current value: {CurrentUsername}", loginPage.GetUsername());
        this.Logger.Verbose("Cleared password, current value: {CurrentPassword}", loginPage.GetPassword());

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
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithoutPassword_ShowsPasswordRequiredError(BrowserOptions options)
    {
        this.InitializeDriver(options);

        var loginPage = new LoginPage(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        var username = "asdf";
        var password = "zxcv";

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
            password, loginPage.GetPassword());

        loginPage.ClearPassword();

        this.Logger.Verbose("Cleared password, current value: {CurrentPassword}", loginPage.GetPassword());

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
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithValidCredentials_RedirectsToInventoryPage(BrowserOptions options)
    {
        this.InitializeDriver(options);

        var loginPage = new LoginPage(this._driver ?? throw new InvalidOperationException("Driver not initialized"));

        var username = loginPage.GetStandardAcceptedUsername();
        var password = loginPage.GetAcceptedPassword();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
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