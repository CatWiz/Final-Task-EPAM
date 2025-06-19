using FinalTask.Config;
using FluentAssertions;

namespace FinalTask;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void SeleniumGridUrlShouldNotBeEmpty()
    {
        string url = TestsConfig.SeleniumGridUrl;

        _ = url.Should().NotBeNullOrWhiteSpace();
    }
}
