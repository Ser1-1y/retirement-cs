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
            Console.WriteLine(Loc.GetId("error-sex"));
            Environment.Exit(1);
        }
        
        var sexLower = sex.ToLower();
        
        if (LocaleData.MaleDict.Any(str => str.Equals(sexLower, StringComparison.CurrentCultureIgnoreCase)))
            return true;
        if (LocaleData.FemaleDict.Any(str => str.Equals(sexLower, StringComparison.CurrentCultureIgnoreCase)))
            return false;

        Console.WriteLine(Loc.GetId("error-sex"));
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

public class ConsoleProgressBar(int total, int widthOfBar = ConsoleProgressBar.DefaultWidthOfBar)
    : IProgressBar
{
    private const ConsoleColor ForeColor = ConsoleColor.Green;
    private const ConsoleColor BkColor = ConsoleColor.Gray;
    private const int DefaultWidthOfBar = 32;
    private const int TextMarginLeft = 3;

    private bool _intited;

    private void Init()
    {
        _lastPosition = 0;

        Console.CursorVisible = false;
        Console.CursorLeft = 0;
        Console.Write("["); 
        Console.CursorLeft = widthOfBar;
        Console.Write("]"); 
        Console.CursorLeft = 1;

        for (var position = 1; position < widthOfBar; position++)
        {
            Console.BackgroundColor = BkColor;
            Console.CursorLeft = position;
            Console.Write(" ");
        }
    }

    public void ShowProgress(int currentCount)
    {
        if (!_intited)
        {
            Init();
            _intited = true;
        }
        DrawTextProgressBar(currentCount);
    }

    private int _lastPosition;

    private void DrawTextProgressBar(int currentCount)
    {
        
        var position = currentCount * widthOfBar / total;
        if (position != _lastPosition)
        {
            _lastPosition = position;
            Console.BackgroundColor = ForeColor;
            Console.CursorLeft = position >= widthOfBar ? widthOfBar - 1 : position;
            Console.Write(" ");
        }

        Console.CursorLeft = widthOfBar + TextMarginLeft;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write($"{currentCount}%" + "    ");
    }
}

public interface IProgressBar
{
    public void ShowProgress(int currentCount);
}