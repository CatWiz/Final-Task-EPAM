

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
    public override void LoginWithNoCredentialsShouldShowUsernameMissingError()
    {
        base.LoginWithNoCredentialsShouldShowUsernameMissingError();
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        base.TestCleanup();
    }
}