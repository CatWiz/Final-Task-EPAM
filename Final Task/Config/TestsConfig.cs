using Microsoft.Extensions.Configuration;

namespace FinalTask.Config;

public static class TestsConfig
{
    private static readonly IConfigurationRoot ConfigRoot = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private static string GetValue(string key)
    {
        return ConfigRoot[key] ?? throw new InvalidOperationException($"{key} is not configured in appsettings.json");
    }
    public static string SeleniumGridUrl => GetValue("Selenium:GridUrl");
    public static string BaseUrl => GetValue("BaseUrl");
    public static string LogOutputPath => GetValue("Logs:FilePath");
    public static string LogOutputLevel => GetValue("Logs:Level");
}
