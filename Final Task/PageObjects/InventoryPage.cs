using FinalTask.Config;
using OpenQA.Selenium;

namespace FinalTask.PageObjects;

public class InventoryPage
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IWebDriver driver;
#pragma warning restore IDE0052 // Remove unread private members

    private static readonly string InventoryPageUrl = $"{TestsConfig.BaseUrl}/inventory.html";

    public InventoryPage(IWebDriver driver)
    {
        this.driver = driver;

        if (driver.Url != InventoryPageUrl)
        {
            throw new InvalidOperationException("Not on the inventory page");
        }
    }

    /// Add inventory page functionality here
}