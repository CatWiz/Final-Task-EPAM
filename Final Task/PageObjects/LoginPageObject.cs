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
    private const string ErrorMessageSelector = "div.error-message-container h3[data-test='error']";

    public LoginPageObject(IWebDriver driver)
    {
        this.driver = driver;

        if (driver.Url != TestsConfig.BaseUrl)
        {
            throw new InvalidOperationException($"Not on the login page, expected {TestsConfig.BaseUrl} but got {driver.Url}");
        }
    }

    public void EnterUsername(string username)
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));
        usernameField.Clear();
        usernameField.SendKeys(username);
    }

    public string? GetUsername()
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));
        return usernameField.GetAttribute("value");
    }

    public void ClearUsername()
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));
        if (!string.IsNullOrEmpty(usernameField.GetAttribute("value")))
        {
            usernameField.SendKeys(Keys.Control + "a" + Keys.Delete);
        }

        _ = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5))
            .Until(d => string.IsNullOrEmpty(this.GetUsername())); // Wait until the field is cleared
    }

    public void EnterPassword(string password)
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));
        passwordField.Clear();
        passwordField.SendKeys(password);
    }

    public string? GetPassword()
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));
        return passwordField.GetAttribute("value");
    }

    public void ClearPassword()
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));
        if (!string.IsNullOrEmpty(passwordField.GetAttribute("value")))
        {
            passwordField.SendKeys(Keys.Control + "a" + Keys.Delete);
        }

        _ = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5))
            .Until(d => string.IsNullOrEmpty(this.GetPassword())); // Wait until the field is cleared
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
            .Pause(TimeSpan.FromSeconds(1)) // Optional pause before clicking
            .Click(loginButton)
            .Pause(TimeSpan.FromSeconds(3)) // Wait for any potential error message or redirection
            .Build();

        actions.Perform();
    }

    public string? GetErrorMessage()
    {
        try
        {
            var errorMessageElement = this.driver.FindElement(By.CssSelector(ErrorMessageSelector));
            return errorMessageElement.Text;
        }
        catch (NoSuchElementException)
        {
            return null; // No error message found
        }
    }
}
