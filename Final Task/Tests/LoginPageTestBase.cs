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
    public virtual void LoginWithNoCredentialsShouldShowUsernameMissingError()
    {
        var driver = this.Driver;

        var loginPage = new LoginPageObject(driver);

        loginPage.EnterUsername("asdf");
        loginPage.EnterPassword("zxcv");

        loginPage.ClearUsername();
        loginPage.ClearPassword();


        loginPage.LoginExpectFailure();

        _ = driver.Url
            .Should().Be(TestsConfig.BaseUrl, "login with no credentials should stay on the login page");

        var errorMessage = loginPage.GetErrorMessage();
        _ = errorMessage
            .Should().Be("Epic sadface: Username is required", $"error message should indicate missing username, username: {loginPage.GetUsername()}, password: {loginPage.GetPassword()}");
    }

    public void Dispose()
    {
        this.Driver.Quit();
        this.Driver.Dispose();
    }
}