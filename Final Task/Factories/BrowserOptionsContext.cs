namespace FinalTask.Factories;

/// <summary>
/// Represents intermediate state for <see cref="BrowserOptionsBuilder"/>
/// </summary>
public record class BrowserOptionsContext
{
    public List<string> Arguments { get; } = [];
    public Dictionary<string, bool> Preferences { get; } = [];
    public Dictionary<string, object> AdditionalOptions { get; } = [];
    public bool Headless { get; set; } = false;
    public bool Maximize { get; set; } = false;
    public bool Incognito { get; set; } = false;
    public (int Width, int Height)? WindowSize { get; set; } = null;
}
