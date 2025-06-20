using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

using BrowserType = FinalTask.Factories.BrowserOptionsBuilder.BrowserType;

namespace FinalTask.Factories;

[DebuggerDisplay("{BrowserType}")]
public class BrowserOptions
{
    public BrowserType BrowserType
    {
        get
        {
            return this.DriverOptions switch
            {
                ChromeOptions => BrowserType.Chrome,
                FirefoxOptions => BrowserType.Firefox,
                EdgeOptions => BrowserType.Edge,
                _ => throw new NotSupportedException("Unsupported browser type")
            };
        }
    }
    public required DriverOptions DriverOptions { get; init; }
    public bool Maximize { get; init; }
}
