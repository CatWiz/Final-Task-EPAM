using System.Reflection;
using FinalTask.Config;
using FinalTask.Extensions;
using FinalTask.Factories;
using FinalTask.PageObjects;
using FluentAssertions;
using OpenQA.Selenium;
using Serilog;

namespace FinalTask.Tests;

[TestClass]
[TestCategory("LoginPage")]
public class LoginPageTests : IDisposable
{
    private readonly ILogger Logger = LoggerFactory.GetLogger();
    private IWebDriver? _driver;

    public TestContext TestContext;

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
        if (this.TestContext is not null && options.DisplayName is not null)
        {
            this.TestContext.Properties["DisplayName"] = $"{this.TestContext.TestName} ({options.DisplayName})";
        }
        this.Logger.Information("Initializing driver for {TestName} for {Browser} browser",
            this.TestContext?.TestName,
            options.DriverOptions.BrowserName);
        this.Logger.LogBrowserOptions(options);

        this._driver = WebDriverFactory.GetDriver(options);
        this._driver.Navigate().GoToUrl(TestsConfig.BaseUrl);
    }

    public LoginPageTests(TestContext testContext)
    {
        this.TestContext = testContext;
    }

    public static string GetTestDisplayName(MethodInfo methodInfo, object[] args)
    {
        var options = (BrowserOptions)args[0];
        return $"{methodInfo.Name} ({options.DisplayName})";
    }

    [TestCleanup]
    public void TestCleanup()
    {
        this._driver?.Quit();
        this._driver?.Dispose();
        this._driver = null;
    }

    [DataTestMethod]
    [TestCategory("EmptyLoginAndPassword")]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property, DynamicDataDisplayName = nameof(GetTestDisplayName))]
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

    [DataTestMethod]
    [TestCategory("EmptyPassword")]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property, DynamicDataDisplayName = nameof(GetTestDisplayName))]
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

    [DataTestMethod]
    [TestCategory("ValidLoginAndPassword")]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property, DynamicDataDisplayName = nameof(GetTestDisplayName))]
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
