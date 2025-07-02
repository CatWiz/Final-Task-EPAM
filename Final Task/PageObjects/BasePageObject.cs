using OpenQA.Selenium;

namespace FinalTask.PageObjects;
public abstract class BasePageObject
{
    protected IWebDriver webDriver;

    public string Url => this.webDriver.Url.TrimEnd('/');
    public string Title => this.webDriver.Title;

    protected BasePageObject(IWebDriver driver)
    {
        ArgumentNullException.ThrowIfNull(driver);
        this.webDriver = driver;
    }

    protected IWebElement FindByCss(string selector)
    {
        return this.webDriver.FindElement(By.CssSelector(selector));
    }
}
