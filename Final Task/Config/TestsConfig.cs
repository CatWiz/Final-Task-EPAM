using Microsoft.Extensions.Configuration;

namespace FinalTask.Config;

public static class TestsConfig
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private static string GetValue(string key)
    {
        return Config[key] ?? throw new InvalidOperationException($"{key} is not configured in appsettings.json");
    }
    public static string SeleniumGridUrl => GetValue("Selenium:GridUrl");
    public static string BaseUrl => GetValue("BaseUrl");
}
