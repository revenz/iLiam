namespace iLiam
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    class EntryPoint
    {

        private static Dictionary<string, string> availableGames;
        private static Dictionary<string, string> availableWatches;
        public static string GameName = null;
        public static string WatchName = null;

        [STAThread]
        public static void Main(string[] args)
        {
#if (DEBUG)
            args = "want to play minecraft".Split(' ');
#endif
            availableGames = ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("game-")).ToDictionary(x => x.Substring(5).Replace("-", " "), x => ConfigurationManager.AppSettings[x]);
            availableWatches = ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith("watch-")).ToDictionary(x => x.Substring(6).Replace("-", " "), x => ConfigurationManager.AppSettings[x]);
            int result = 10;
            if (ConfigurationManager.AppSettings["required"] != null)
            {
                int.TryParse(ConfigurationManager.AppSettings["required"], out result);
            }
            MainWindow.Required = result;
            if ((args != null) && (args.Length != 0))
            {
                string str = string.Join(" ", args).ToLower();
                if (str.StartsWith("want to play "))
                {
                    GameName = str.Substring("want to play ".Length);
                    if (!availableGames.ContainsKey(GameName))
                    {
                        Console.WriteLine("I do not know the game: " + str.Substring("want to play ".Length).Replace(" ", "-"));
                        return;
                    }
                }
                else if (str.StartsWith("want to watch "))
                {
                    WatchName = str.Substring("want to watch ".Length);
                    if (!availableWatches.ContainsKey(WatchName))
                    {
                        Console.WriteLine("I do not know the watch: " + str.Substring("want to watch ".Length).Replace(" ", "-"));
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("You must enter: I want to play [GAMENAME]");
                    Console.WriteLine("You must enter: I want to watch [WATCHNAME]");
                    return;
                }
                App app1 = new App();
                app1.InitializeComponent();
                app1.Run();
            }
            else
            {
                Console.WriteLine("Liam, you must enter a game.");
            }
        }

        public static void RunGame()
        {
            if (WatchName != null)
            {
                if (availableWatches.ContainsKey(WatchName))
                {
                    Process.Start(availableWatches[WatchName]);
                    Environment.Exit(0);
                }
            }
            else
            {
                if (availableGames.ContainsKey(GameName))
                {
                    Process.Start(availableGames[GameName]);
                    Environment.Exit(0);
                }
            }
        }        
    }
}
