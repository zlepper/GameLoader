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
        private readonly string gamesPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameLoader", "games.json");
        private readonly string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameLoader", "config.json");

        public List<Game> LoadGames()
        {
            if (!File.Exists(gamesPath)) return new List<Game>();
            string json = File.ReadAllText(gamesPath);
            return JsonConvert.DeserializeObject<List<Game>>(json);
        }

        public void SaveGames(List<Game> games)
        {
            new FileInfo(gamesPath).Directory?.Create();
            try
            {
                File.Delete(gamesPath);
            }
            catch (FileNotFoundException) {} catch(DirectoryNotFoundException) { }
            string json = JsonConvert.SerializeObject(games, Formatting.Indented);
            File.WriteAllText(gamesPath, json);
        }

        public void SaveConfig(Config config)
        {
            try
            {
                File.Delete(configPath);
            }
            catch (FileNotFoundException)
            {
            }
            catch (DirectoryNotFoundException)
            {
                var f = new FileInfo(configPath);
                var d = f.Directory;
                if (d != null && !d.Exists)
                {
                    d.Create();
                }
            }
            try
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            catch (DirectoryNotFoundException)
            {
                var f = new FileInfo(configPath);
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
            if(!File.Exists(configPath)) return new Config();
            string json = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<Config>(json);
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
