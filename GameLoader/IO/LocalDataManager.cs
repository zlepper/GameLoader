using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameLoader.IO
{
    public class LocalDataManager
    {
        private static readonly string GamesPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameLoader", "games.json");
        private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameLoader", "config.json");

        public List<Game> LoadGames()
        {
            if (!File.Exists(GamesPath)) return new List<Game>();
            string json = File.ReadAllText(GamesPath);
            return JsonConvert.DeserializeObject<List<Game>>(json);
        }

        public void SaveGames(List<Game> games)
        {
            new FileInfo(GamesPath).Directory?.Create();
            try
            {
                File.Delete(GamesPath);
            }
            catch (FileNotFoundException) {} catch(DirectoryNotFoundException) { }
            string json = JsonConvert.SerializeObject(games, Formatting.Indented);
            File.WriteAllText(GamesPath, json);
        }

        public void SaveConfig(Config config)
        {
            try
            {
                File.Delete(ConfigPath);
            }
            catch (FileNotFoundException)
            {
            }
            catch (DirectoryNotFoundException)
            {
                var f = new FileInfo(ConfigPath);
                var d = f.Directory;
                if (d != null && !d.Exists)
                {
                    d.Create();
                }
            }
            try
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            catch (DirectoryNotFoundException)
            {
                var f = new FileInfo(ConfigPath);
                var d = f.Directory;
                if (d != null && !d.Exists)
                {
                    d.Create();
                }
                SaveConfig(config);
            }
        }

        public Config LoadConfig()
        {
            if(!File.Exists(ConfigPath)) return new Config();
            string json = File.ReadAllText(ConfigPath);
            return JsonConvert.DeserializeObject<Config>(json);
        }

        /// <summary>
        /// Resets the application state to completely new. Useful if you say change a hard disk drive
        /// 
        /// And before you ask me what somebody would want to do that, look at this ticket: 
        /// https://github.com/zlepper/GameLoader/issues/10
        /// </summary>
        public static void ResetApplication()
        {
            File.Delete(GamesPath);
            File.Delete(ConfigPath);
        }

    }

    /// <summary>
    /// A strongly typed class of all the configs the app has
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The output path where GameLoader stores all games that are activated
        /// </summary>
        public string OutputPath =
            Path.Combine(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)), "GameLoader");

        /// <summary>
        /// Indicates if this is the first time we are running
        /// Will be used to do some initial setup like games discovery
        /// </summary>
        public bool FirstRun = true;

        /// <summary>
        /// A list of folders where games are installed, such as the SteamApp directory
        /// </summary>
        public List<string> GamesFolders = new List<string>(); 
    }
}
