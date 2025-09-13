using IniParser;

namespace Retirement;

public static class Config
{
    private const string ConfigPath = "config.ini";

    public static string GetConfig()
    {
        if (!File.Exists(ConfigPath)) return "Null";

        var parser = new FileIniDataParser();
        var data = parser.ReadFile(ConfigPath);
        var language = data["config"]["language"];
        
        return language;
    }

    public static void SetConfig(string locale)
    {
        File.WriteAllText(ConfigPath, "[config]\n" + $"language={locale}\n");
    }
}