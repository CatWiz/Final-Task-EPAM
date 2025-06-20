

using FinalTask.Factories;

namespace FinalTask.Tests;

[TestClass]
public class LoginPageChromeTests : LoginPageTestsBase
{
    public LoginPageChromeTests() : base(WebDriverType.Chrome)
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