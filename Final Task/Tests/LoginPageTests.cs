using FinalTask.Config;
using FinalTask.PageObjects;
using FluentAssertions;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageTests(TestContext testContext) : BaseTest(testContext), IDisposable
{
    public static IEnumerable<object[]> BrowserOptions => BrowserOptionsProvider.AllOptionsNames
        .Select(name => new object[] { name });

    private LoginPage _loginPage = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        this._loginPage = new LoginPage(this.Driver ?? throw new InvalidOperationException("Driver not initialized"));
    }

    [TestMethod]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithoutUsername_ShowsUsernameRequiredError(string options)
    {
        var username = "asdf";
        var password = "zxcv";

        this._loginPage.EnterUsername(username);
        this._loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, this._loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
            password, this._loginPage.GetPassword());

        this._loginPage.ClearUsername();
        this._loginPage.ClearPassword();

        this.Logger.Verbose("Cleared username, current value: {CurrentUsername}", this._loginPage.GetUsername());
        this.Logger.Verbose("Cleared password, current value: {CurrentPassword}", this._loginPage.GetPassword());

        this._loginPage.LoginExpectFailure();

        this.Logger.Information("Tried logging in with cleared credentials, current URL: {CurrentUrl}",
            this._loginPage.Url);

        _ = this._loginPage.Url
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = this._loginPage.GetErrorMessage();
        this.Logger.Information("Verifying error message after login attempt, error message: {ErrorMessage}",
            errorMessage);

        _ = errorMessage
            .Should().Be("Epic sadface: Username is required",
                        $"error message should indicate missing username, username: {this._loginPage.GetUsername()}, password: {this._loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithoutPassword_ShowsPasswordRequiredError(string options)
    {
        var username = "asdf";
        var password = "zxcv";

        this._loginPage.EnterUsername(username);
        this._loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, this._loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
            password, this._loginPage.GetPassword());

        this._loginPage.ClearPassword();

        this.Logger.Verbose("Cleared password, current value: {CurrentPassword}", this._loginPage.GetPassword());

        this._loginPage.LoginExpectFailure();

        this.Logger.Information("Tried logging in with cleared password, current URL: {CurrentUrl}",
            this._loginPage.Url);

        _ = this._loginPage.Url
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = this._loginPage.GetErrorMessage();
        this.Logger.Information("Verifying error message after login attempt, error message: {ErrorMessage}",
            errorMessage);

        _ = errorMessage
            .Should().Be("Epic sadface: Password is required",
                        $"error message should indicate missing password, username: {this._loginPage.GetUsername()}, password: {this._loginPage.GetPassword()}");
    }

    [TestMethod]
    [DynamicData(nameof(BrowserOptions), DynamicDataSourceType.Property)]
    public void Login_WithValidCredentials_RedirectsToInventoryPage(string options)
    {
        var username = this._loginPage.GetStandardAcceptedUsername();
        var password = this._loginPage.GetAcceptedPassword();

        this._loginPage.EnterUsername(username);
        this._loginPage.EnterPassword(password);

        this.Logger.Verbose("Entered username {username}, current value: {CurrentUsername}",
            username, this._loginPage.GetUsername());
        this.Logger.Verbose("Entered password {password}, current value: {CurrentPassword}",
            password, this._loginPage.GetPassword());

        _ = this._loginPage.LoginExpectSuccess();

        this.Logger.Information("Login successful, current URL: {CurrentUrl}", this._loginPage.Url);
        this.Logger.Information("Current page title: {PageTitle}", this._loginPage.Title);

        _ = this._loginPage.Url
            .Should().Be($"{TestsConfig.BaseUrl}/inventory.html", "login with valid credentials should redirect to the inventory page");
        _ = this._loginPage.Title
            .Should().Be("Swag Labs", "page title should be 'Swag Labs' after successful login");
    }
}
