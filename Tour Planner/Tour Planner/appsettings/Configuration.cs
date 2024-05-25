using Microsoft.Extensions.Configuration;

public static class Configuration
{
    public static IConfigurationRoot Config { get; private set; }

    static Configuration()
    {
        Config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public static AppSettings GetAppSettings()
    {
        var appSettings = new AppSettings();
        Config.GetSection("ConnectionStrings").Bind(appSettings);
        Config.GetSection("OpenRouteService").Bind(appSettings);
        return appSettings;
    }
}