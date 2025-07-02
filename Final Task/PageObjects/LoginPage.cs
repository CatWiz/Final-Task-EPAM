using FinalTask.Config;
using FinalTask.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace FinalTask.PageObjects;

public class LoginPage : BasePageObject
{
    #region Selectors and getters

    private IWebElement UsernameField => this.FindByCss("input[type='text']#user-name.form_input");
    private IWebElement PasswordField => this.FindByCss("input[type='password']#password.form_input");
    private IWebElement LoginButton => this.FindByCss("input[type='submit']#login-button.submit-button");
    private IWebElement ErrorMessageDisplay => this.FindByCss("div.error-message-container h3[data-test='error']");
    private IWebElement AcceptedUsernamesList => this.FindByCss("div.login_credentials");
    private IWebElement AcceptedPasswordsList => this.FindByCss("div.login_password");

    #endregion

    public LoginPage(IWebDriver driver) : base(driver)
    {
        if (this.Url != TestsConfig.BaseUrl)
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
        this.ClearUsername();
        this.UsernameField.SendKeys(username);
    }

    /// <summary>
    /// Clears the password input field and enters the specified password.
    /// </summary>
    /// <param name="password">Password to enter</param>
    public void EnterPassword(string password)
    {
        this.ClearPassword();
        this.PasswordField.SendKeys(password);
    }

    /// <summary>
    /// Gets the current value of the username input field.
    /// If value is not set, returns an empty string.
    /// </summary>
    /// <returns>Current value of the input field</returns>
    public string GetUsername()
    {
        return this.UsernameField.GetAttribute("value") ?? string.Empty;
    }

    /// <summary>
    /// Gets the current value of the password input field.
    /// If value is not set, returns an empty string.
    /// </summary>
    /// <returns>Current value of the input field</returns>
    public string GetPassword()
    {
        return this.PasswordField.GetAttribute("value") ?? string.Empty;
    }

    /// <summary>
    /// Clears the username input field.
    /// </summary>
    public void ClearUsername()
    {
        this.UsernameField.ClearBySelectAndDelete();

        _ = new WebDriverWait(this.webDriver, TimeSpan.FromSeconds(5))
            .Until(d => string.IsNullOrEmpty(this.GetUsername())); // Wait until the field is cleared
    }

    /// <summary>
    /// Clears the password input field.
    /// </summary>
    public void ClearPassword()
    {
        this.PasswordField.ClearBySelectAndDelete();

        _ = new WebDriverWait(this.webDriver, TimeSpan.FromSeconds(5))
            .Until(d => string.IsNullOrEmpty(this.GetPassword())); // Wait until the field is cleared
    }

    /// <summary>
    /// Clicks the login button expecting to be redirected to the inventory page.
    /// </summary>
    /// <returns>Instance of <see cref="InventoryPage"/> if login is successful</returns>
    /// <exception cref="InvalidOperationException">Thrown if login fails or does not redirect to the inventory page</exception>
    public InventoryPage LoginExpectSuccess()
    {
        this.LoginButton.Click();

        _ = new WebDriverWait(this.webDriver, TimeSpan.FromSeconds(10))
            .Until(d => d.Url.Contains("/inventory.html"));

        return new InventoryPage(this.webDriver);
    }

    /// <summary>
    /// Clicks the login button expecting a failure (e.g., missing credentials error).
    /// </summary>
    public void LoginExpectFailure()
    {
        new Actions(this.webDriver)
            .Pause(TimeSpan.FromSeconds(1)) // Optional pause before clicking
            .Click(this.LoginButton)
            .Pause(TimeSpan.FromSeconds(3)) // Wait for any potential error message or redirection
            .Build()
            .Perform();
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
            return this.ErrorMessageDisplay.Text;
        }
        catch (NoSuchElementException)
        {
            return null; // No error message found
        }
    }

    /// <summary>
    /// Retrieves all usernames that can be used to log in.
    /// </summary>
    /// <returns>Enumerable of accepted usernames</returns>
    public IEnumerable<string> GetAllAcceptedUsernames()
    {
        return this.AcceptedUsernamesList.Text
            .Split(['\n'], StringSplitOptions.RemoveEmptyEntries)
            .Skip(1) // The first line is a header, skip it
            .Select(line => line.Trim());
    }

    /// <summary>
    /// Retrieves the first username from the list of accepted usernames.
    /// This username allows logging in without any side effects.
    /// </summary>
    /// <returns>Standard username for login</returns>
    public string GetStandardAcceptedUsername()
    {
        return this.GetAllAcceptedUsernames().First();
    }

    /// <summary>
    /// Retrieves all accepted passwords that can be used to log in.
    /// </summary>
    /// <returns>Enumerable of accepted passwords</returns>
    public IEnumerable<string> GetAllAcceptedPasswords()
    {
        return this.AcceptedPasswordsList.Text
            .Split(['\n'], StringSplitOptions.RemoveEmptyEntries)
            .Skip(1) // The first line is a header, skip it
            .Select(line => line.Trim());
    }

    /// <summary>
    /// Retrieves the only accepted password from the list of accepted passwords.
    /// </summary>
    /// <returns></returns>
    public string GetAcceptedPassword()
    {
        return this.GetAllAcceptedPasswords().First();
    }
}
