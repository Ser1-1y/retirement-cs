namespace Retirement;

public static class Functions
{
    public static bool IsReadyToRetire(string? ageInput, string? sex)
    {
        if (int.TryParse(ageInput, out var age))
            return IsMale(sex) ? age >= (int)RetirementAge.Male : age >= (int)RetirementAge.Female;
        
        Console.WriteLine(Loc.GetId("error-age-number"));
        Environment.Exit(1);

        return IsMale(sex) ? age >= (int)RetirementAge.Male : age >= (int)RetirementAge.Female;
    }

    private static bool IsMale(string? sex)
    {
        if (sex == null)
        {
            Console.WriteLine(Loc.GetId("error_sex"));
            Environment.Exit(1);
        }
        
        var sexLower = sex.ToLower();
        
        if (LocaleData.MaleDict.Any(str => str.Equals(sexLower, StringComparison.CurrentCultureIgnoreCase)))
            return true;
        if (LocaleData.FemaleDict.Any(str => str.Equals(sexLower, StringComparison.CurrentCultureIgnoreCase)))
            return false;

        Console.WriteLine(Loc.GetId("error_sex"));
        Environment.Exit(1);
        return false;
    }

    public static Locale GetLocale(string? languageInput)
    {
        if (string.IsNullOrWhiteSpace(languageInput))
        {
            Console.WriteLine(Loc.GetId("error-language"));
            Environment.Exit(1);
        }

        var normalized = languageInput.Trim();

        if (Enum.TryParse<Locale>(normalized, ignoreCase: true, out var parsed))
        {
            Loc.CurrentLocale = parsed;
            Config.SetConfig(parsed.ToString());
            return parsed;
        }

        foreach (var pair in LocaleData.LocaleMappings.Where(p => p.Value.Contains(normalized)))
        {
            Loc.CurrentLocale = pair.Key;
            Config.SetConfig(pair.Key.ToString());
            return pair.Key;
        }

        Console.WriteLine(Loc.GetId("error-language"));
        Environment.Exit(1);
        return Locale.English;
    }
}