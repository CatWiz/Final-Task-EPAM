

using OpenQA.Selenium;

namespace FinalTask.Extensions;

public static class IWebElementExtensions
{
    /// <summary>
    /// Clears the input field by selecting all text and deleting it.
    /// This should be used in place of <see cref="IWebElement.Clear()"/>> to mimic the way a real user would clear the field.
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