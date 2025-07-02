using FinalTask.Config;
using OpenQA.Selenium;

namespace FinalTask.PageObjects;

public class InventoryPage : BasePageObject
{
    private static readonly string InventoryPageUrl = $"{TestsConfig.BaseUrl}/inventory.html";

    public InventoryPage(IWebDriver driver) : base(driver)
    {
        if (this.Url != InventoryPageUrl)
        {
            throw new InvalidOperationException("Not on the inventory page");
        }
    }

    /// Add inventory page functionality here
}
