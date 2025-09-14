using System.Text.Json;

namespace Retirement;

public static class LocaleData
{
    public static readonly Dictionary<Locale, Dictionary<string, string>> LocaleStrings;

    public static readonly List<string> MaleDict = ["1. Male", "M", "1. мужской", "М", "1", "1."];
    public static readonly List<string> FemaleDict = ["2. Female", "F", "2. женский", "Ж", "2", "2."];

    static LocaleData()
    {
        var json = File.ReadAllText("locale.json");

        var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)!;

        LocaleStrings = data.ToDictionary(
            kvp => Enum.Parse<Locale>(kvp.Key),
            kvp => kvp.Value
        );
    }
    
    public static readonly Dictionary<Locale, string[]> LocaleMappings = new()
    {
        { Locale.English, ["English", "1. English"] },
        { Locale.Russian, ["Russian", "2. Russian"] },
    };
}
public static class Loc
{
    public static Locale CurrentLocale { get; set; } = Locale.English;

    public static string GetId(string key)
    {
        return LocaleData.LocaleStrings[CurrentLocale].TryGetValue(key, out var value)
            ? value
            : $"[{key}]";
    }
    public static bool HasId(string key)
    {
        return LocaleData.LocaleStrings[CurrentLocale].TryGetValue(key, out _);
    }
}
