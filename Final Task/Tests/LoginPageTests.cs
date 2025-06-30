using FinalTask.Config;
using FinalTask.PageObjects;
using FluentAssertions;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageTests(TestContext testContext) : BaseTest(testContext), IDisposable
{
    public static IEnumerable<object[]> BrowserOptions
    {
        get
        {
            yield return [nameof(BrowserOptionsProvider.ChromeHeadless)];
            yield return [nameof(BrowserOptionsProvider.FirefoxHeadless)];
            yield return [nameof(BrowserOptionsProvider.EdgeHeadless)];

            yield return [nameof(BrowserOptionsProvider.ChromeHeadlessTiny)];
            yield return [nameof(BrowserOptionsProvider.FirefoxHeadlessTiny)];
            yield return [nameof(BrowserOptionsProvider.EdgeHeadlessTiny)];
        }
    }

    [TestMethod]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithoutUsername_ShowsUsernameRequiredError(string options)
    {
        var loginPage = new LoginPage(this.Driver ?? throw new InvalidOperationException("Driver not initialized"));
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
            this.Driver.Url);

        _ = this.Driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        this.Logger.Information("Verifying error message after login attempt, error message: {ErrorMessage}",
            errorMessage);

        _ = errorMessage
            .Should().Be("Epic sadface: Username is required", $"error message should indicate missing username, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithoutPassword_ShowsPasswordRequiredError(string options)
    {
        var loginPage = new LoginPage(this.Driver ?? throw new InvalidOperationException("Driver not initialized"));

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
            this.Driver.Url);

        _ = this.Driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        this.Logger.Information("Verifying error message after login attempt, error message: {ErrorMessage}",
            errorMessage);

        _ = errorMessage
            .Should().Be("Epic sadface: Password is required", $"error message should indicate missing password, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithValidCredentials_RedirectsToInventoryPage(string options)
    {
        var loginPage = new LoginPage(this.Driver ?? throw new InvalidOperationException("Driver not initialized"));

        var username = loginPage.GetStandardAcceptedUsername();
        var password = loginPage.GetAcceptedPassword();

        loginPage.EnterUsername(username);
        loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
            password, loginPage.GetPassword());

        _ = loginPage.LoginExpectSuccess();

        this.Logger.Information("Login successful, current URL: {CurrentUrl}", this.Driver.Url);
        this.Logger.Information("Current page title: {PageTitle}", this.Driver.Title);

        _ = this.Driver.Url
            .Should().Be($"{TestsConfig.BaseUrl}/inventory.html", "login with valid credentials should redirect to the inventory page");
        _ = this.Driver.Title
            .Should().Be("Swag Labs", "page title should be 'Swag Labs' after successful login");
    }
}
