using System.Diagnostics;

namespace Retirement;

public static class Program
{
    public static void Main(string[] args)
    {
        Stopwatch stopwatch = null!;
        var help = false;
        var debug = false;
        
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "help":
                    help = true;
                    break;
                case "--debug":
                    debug = true;
                    stopwatch = Stopwatch.StartNew();
                    break;
            }
        }
        if (help)
        {
            Console.WriteLine("Usage: Retirement.exe [options]\n\n" +
                              "Options:\n" +
                              "  help      List of options.\n" +
                              "  --debug   Show debug information.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        var savedLang = Config.GetConfig();
        if (savedLang == "Null")
            ChangeLanguage();
        else
        {
            Functions.GetLocale(savedLang);
        }
        
        Console.Clear();
        
        string? age;
        if (debug)
        {
            while (true)
            {
                Console.WriteLine($"{Loc.GetId("welcome")}\n" +
                                  $"{Loc.GetId("enter-age")}");
                age = Console.ReadLine();

                if (age == null) continue;

                if (age.Equals("ChangeLanguage", StringComparison.OrdinalIgnoreCase))
                {
                    ChangeLanguage();
                    continue;
                }

                break;
            }
        }
        else
        {
            Console.WriteLine($"{Loc.GetId("welcome")}\n" +
                              $"{Loc.GetId("enter-age")}");
            age = Console.ReadLine();
        }
        
        var sex = GraphicChoice([
            Loc.GetId("male-option"),
            Loc.GetId("female-option")
        ], Loc.GetId("enter-sex"));

        const int total = 100;
        var progressBar = new ConsoleProgressBar(total);
        
        Console.WriteLine(Loc.GetId("pension-check-start"));
        Thread.Sleep(Random.Shared.Next(200, 800));
        
        for (var i = 0; i <= total; i++)
        {
            progressBar.ShowProgress(i);
            Thread.Sleep(Random.Shared.Next(0, 200));
        }

        Thread.Sleep(500);
        Console.Clear();
        
        Console.Clear();
        
        var readyId = Loc.GetId(Functions.IsReadyToRetire(age, sex) ? "ready" : "not-ready");
        foreach (var character in readyId)
        {
            Console.Write(character);
            Thread.Sleep(Random.Shared.Next(0, 200));
        }
        Console.WriteLine();

        if (!debug) return;
        stopwatch.Stop();
        Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds} from the start.");
    }

    private static void ChangeLanguage()
    {
        var languageInput = GraphicChoice([
            "1. English",
            "2. Russian"
        ], "Please choose your language:");

        Functions.GetLocale(languageInput);
    }
    
    /// <summary>
    /// Presents a user with a graphic selection of some options.
    /// </summary>
    /// <param name="options">String array of options displayed to the user.</param>
    /// <param name="title">Optional title for the options.</param>
    /// <returns>String of a selected option</returns>
    private static string GraphicChoice(string[] options, string title = "")
    {
        var selected = 0;
        ConsoleKey key;

        var (left, top) = Console.GetCursorPosition();
        Console.CursorVisible = false;
        
        do
        {
            Console.SetCursorPosition(left, top);
            
            if (title != "")
                Console.Write(title + "\n");
            
            for (var i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"> {options[i]} <");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}  ");
                }
            }

            var keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            selected = key switch
            {
                ConsoleKey.UpArrow or ConsoleKey.LeftArrow => (selected - 1 + options.Length) % options.Length,
                ConsoleKey.DownArrow or ConsoleKey.RightArrow => (selected + 1) % options.Length,
                _ => selected
            };
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        Console.Clear();
        return options[selected];
    }
}