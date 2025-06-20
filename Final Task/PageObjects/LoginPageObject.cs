using FinalTask.Config;
using FinalTask.Extensions;
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

        if (driver.Url.TrimEnd('/') != TestsConfig.BaseUrl)
        {
            throw new InvalidOperationException($"Not on the login page, expected {TestsConfig.BaseUrl} but got {driver.Url}");
        }
    }

    /// <summary>
    /// Clears the username input field and enters the specified username.
    /// </summary>
    /// <param name="username">Username to enter</param>
    public void EnterUsername(string username)
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));

        usernameField.ClearBySelectAndDelete();
        usernameField.SendKeys(username);
    }

    /// <summary>
    /// Gets the current value of the username input field.
    /// If value is not set, returns an empty string.
    /// </summary>
    /// <returns>Current value of the input field</returns>
    public string GetUsername()
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));
        return usernameField.GetAttribute("value") ?? string.Empty;
    }

    /// <summary>
    /// Clears the username input field.
    /// </summary>
    public void ClearUsername()
    {
        var usernameField = this.driver.FindElement(By.CssSelector(UsernameFieldSelector));

        usernameField.ClearBySelectAndDelete();

        _ = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5))
            .Until(d => string.IsNullOrEmpty(this.GetUsername())); // Wait until the field is cleared
    }

    /// <summary>
    /// Clears the password input field and enters the specified password.
    /// </summary>
    /// <param name="password">Password to enter</param>
    public void EnterPassword(string password)
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));

        passwordField.ClearBySelectAndDelete();
        passwordField.SendKeys(password);
    }

    /// <summary>
    /// Gets the current value of the password input field.
    /// If value is not set, returns an empty string.
    /// </summary>
    /// <returns>Current value of the input field</returns>
    public string GetPassword()
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));
        return passwordField.GetAttribute("value") ?? string.Empty;
    }

    /// <summary>
    /// Clears the password input field.
    /// </summary>
    public void ClearPassword()
    {
        var passwordField = this.driver.FindElement(By.CssSelector(PasswordFieldSelector));

        passwordField.ClearBySelectAndDelete();

        _ = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5))
            .Until(d => string.IsNullOrEmpty(this.GetPassword())); // Wait until the field is cleared
    }

    /// <summary>
    /// Clicks the login button expecting to be redirected to the inventory page.
    /// </summary>
    /// <returns>Instance of <see cref="InventoryPageObject"/> if login is successful</returns>
    /// <exception cref="InvalidOperationException">Thrown if login fails or does not redirect to the inventory page</exception>
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

    /// <summary>
    /// Clicks the login button expecting a failure (e.g., missing credentials error).
    /// </summary>
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

    /// <summary>
    /// Retrieves the error message displayed on the login page.
    /// If error display element is not found, returns null.
    /// </summary>
    /// <returns>Error message text if present, otherwise null</returns>
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
