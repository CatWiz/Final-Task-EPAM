

using FinalTask.Factories;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageFirefoxTests : LoginPageTestsBase
{
    public LoginPageFirefoxTests() : base(WebDriverType.Firefox)
    {
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
    }

    [TestMethod]
    public override void LoginAfterClearingCredentialsShouldShowUsernameMissingError()
    {
        base.LoginAfterClearingCredentialsShouldShowUsernameMissingError();
    }

    [TestMethod]
    public override void LoginAfterClearingPasswordShouldShowPasswordMissingError()
    {
        base.LoginAfterClearingPasswordShouldShowPasswordMissingError();
    }

    [TestMethod]
    public override void LoginWithValidCredentialsShouldRedirectToInventoryPage()
    {
        base.LoginWithValidCredentialsShouldRedirectToInventoryPage();
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        base.TestCleanup();
    }
}