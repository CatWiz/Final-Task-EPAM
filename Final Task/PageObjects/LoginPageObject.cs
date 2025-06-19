using FinalTask.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace FinalTask.PageObjects;

public class LoginPageObject
{
    private readonly IWebDriver driver;
    private const string UsernameFieldSelector = "input[type='text']#user-name.form_input";
    private const string PasswordFieldSelector = "input[type='password']#password.form_input";
    private const string LoginButtonSelector = "input[type='submit']#login-button.submit-button";

    public LoginPageObject(IWebDriver driver)
    {
        this.driver = driver;

        if (driver.Url != TestsConfig.BaseUrl)
        {
            throw new InvalidOperationException("Not on the login page");
        }
    }

    public void EnterUsername(string username)
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));
        usernameField.Clear();
        usernameField.SendKeys(username);
    }

    public void ClearUsername()
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));
        usernameField.Clear();
    }

    public void EnterPassword(string password)
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));
        passwordField.Clear();
        passwordField.SendKeys(password);
    }

    public void ClearPassword()
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));
        passwordField.Clear();
    }

    public InventoryPageObject LoginExpectSuccess()
    {
        var loginButton = this.driver.FindElement(By.CssSelector(LoginButtonSelector));
        loginButton.Click();

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(10));
        var success = wait.Until(d => d.Url.Contains("/inventory.html"));

        return success
            ? new InventoryPageObject(this.driver)
            : throw new InvalidOperationException("Login failed or did not redirect to inventory page");
    }

    public void LoginExpectFailure()
    {
        var loginButton = this.driver.FindElement(By.CssSelector(LoginButtonSelector));

        var actions = new Actions(this.driver)
            .Click(loginButton)
            .Pause(TimeSpan.FromSeconds(5)) // Wait for any potential error message or redirection
            .Build();

        actions.Perform();
    }
}
