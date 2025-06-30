using System.Reflection;
using FinalTask.Config;
using FinalTask.Extensions;
using FinalTask.Factories;
using OpenQA.Selenium;
using Serilog;

namespace FinalTask.Tests;

/// <summary>
/// Base class for all test classes.
/// Implements web driver initialization and cleanup, as well as logger initialization.
/// </summary>
public class BaseTest : IDisposable
{
    protected readonly ILogger Logger = LoggerFactory.GetLogger();
    protected IWebDriver? Driver { get; private set; }
    private readonly TestContext _testContext;

    private void InitializeDriver(string optionsName)
    {
        this.Logger.Information("Initializing with {options} options", optionsName);

        var providerType = typeof(BrowserOptionsProvider);
        var property = providerType.GetProperty(optionsName, BindingFlags.Public | BindingFlags.Static);
        if (property is null)
        {
            this.Logger.Error("Could not find property {PropertyName} in {TypeName}",
                optionsName, providerType.FullName);
            throw new InvalidOperationException($"Could not find property {optionsName} in {providerType.FullName}");
        }

        try
        {
            var options = (BrowserOptions?)property.GetValue(null);
            if (options is null)
            {
                this.Logger.Error("Property {PropertyName} returned null", optionsName);
                throw new InvalidOperationException($"Property {optionsName} returned null");
            }

            this.Logger.LogBrowserOptions(options);
            this.Driver = WebDriverFactory.GetDriver(options);
            this.Driver.Navigate().GoToUrl(TestsConfig.BaseUrl);
        }
        catch (InvalidCastException ex)
        {
            this.Logger.Error(ex, "Failed to cast property {PropertyName} to BrowserOptions", optionsName);
            throw new InvalidOperationException($"Failed to cast property {optionsName} to BrowserOptions", ex);
        }
    }

    public BaseTest(TestContext testContext)
    {
        ArgumentNullException.ThrowIfNull(testContext);
        this._testContext = testContext;
    }

    [TestInitialize]
    public void Initialize()
    {
        this.Logger.Information("Starting test {testName}", this._testContext.TestName);
        if (this._testContext.TestData is null || this._testContext.TestData.Length == 0)
        {
            this.Logger.Error("Test data is null or empty for test {testName}", this._testContext.TestName);
            throw new InvalidOperationException("Test data is null or empty");
        }

        if (this._testContext.TestData[0] is not string optionsName)
        {
            this.Logger.Error("First item in test data is not a string for test {testName}", this._testContext.TestName);
            throw new InvalidOperationException("First element of test data must be a string");
        }

        this.InitializeDriver(optionsName);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (this.Driver is not null)
        {
            this.Logger.Information("Quitting driver for {testName} test", this._testContext.TestName);
            this.Driver.Quit();
            this.Driver.Dispose();
            this.Driver = null;
        }
    }

    public virtual void Dispose()
    {
        this.Cleanup();
    }
}
