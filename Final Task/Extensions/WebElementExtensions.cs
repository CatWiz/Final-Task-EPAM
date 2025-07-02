

using OpenQA.Selenium;

namespace FinalTask.Extensions;

public static class WebElementExtensions
{
    /// <summary>
    /// Clears the input field by selecting all text and deleting it.
    /// This should be used in place of <see cref="IWebElement.Clear()"/>>
    /// to ensure that input events are triggered for that input field.
    /// </summary>
    /// <param name="element">Input element to clear</param>
    /// <exception cref="ArgumentNullException">Thrown if the element is null</exception>
    public static void ClearBySelectAndDelete(this IWebElement element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element), "Element cannot be null");
        }

        element.SendKeys(Keys.Control + "a" + Keys.Delete);
    }
}
