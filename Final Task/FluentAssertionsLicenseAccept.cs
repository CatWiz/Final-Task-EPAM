[assembly: FluentAssertions.Extensibility.AssertionEngineInitializer(
    typeof(AssertionEngineInitializer),
    nameof(AssertionEngineInitializer.AcceptLicense))]

internal static class AssertionEngineInitializer
{
    /// <summary>
    /// Suppress soft warning about FluentAssertions requiring a license for commercial use.
    /// This project is not being used commercially.
    /// </summary>
    public static void AcceptLicense()
    {
        FluentAssertions.License.Accepted = true;
    }
}
