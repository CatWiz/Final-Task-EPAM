using FinalTask.Config;
using FinalTask.Factories;
using FinalTask.PageObjects;
using FluentAssertions;
using OpenQA.Selenium;

namespace FinalTask.Tests;

public abstract class LoginPageTestsBase : IDisposable
{
    protected readonly IWebDriver Driver;

    public LoginPageTestsBase(WebDriverType type)
    {
        this.Driver = WebDriverFactory.GetDriver(type);
    }

    public virtual void TestInitialize()
    {
        this.Driver.Navigate().GoToUrl(TestsConfig.BaseUrl);
    }

    public virtual void TestCleanup()
    {
        this.Driver.Quit();
    }
    public virtual void LoginAfterClearingCredentialsShouldShowUsernameMissingError()
    {
        var driver = this.Driver;

        var loginPage = new LoginPageObject(driver);

        loginPage.EnterUsername("asdf");
        loginPage.EnterPassword("zxcv");

        loginPage.ClearUsername();
        loginPage.ClearPassword();


        loginPage.LoginExpectFailure();

        _ = driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        _ = errorMessage
            .Should().Be("Epic sadface: Username is required", $"error message should indicate missing username, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    public virtual void LoginAfterClearingPasswordShouldShowPasswordMissingError()
    {
        var driver = this.Driver;

        var loginPage = new LoginPageObject(driver);

        loginPage.EnterUsername("asdf");
        loginPage.EnterPassword("zxcv");

        loginPage.ClearPassword();

        loginPage.LoginExpectFailure();

        _ = driver.Url.TrimEnd('/')
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        _ = errorMessage
            .Should().Be("Epic sadface: Password is required", $"error message should indicate missing password, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    public virtual void LoginWithValidCredentialsShouldRedirectToInventoryPage()
    {
        var driver = this.Driver;

        var loginPage = new LoginPageObject(driver);

        loginPage.EnterUsername("standard_user");
        loginPage.EnterPassword("secret_sauce");


        _ = loginPage.LoginExpectSuccess();

        _ = driver.Url
            .Should().Be($"{TestsConfig.BaseUrl}/inventory.html", "login with valid credentials should redirect to the inventory page");
    }

    public void Dispose()
    {
        this.Driver.Quit();
        this.Driver.Dispose();
    }
}