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
            }
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(configPath, json);
        }

        public Config LoadConfig()
        {
            if(!File.Exists(configPath)) return new Config();
            string json = File.ReadAllText(configPath);
            return JsonConvert.DeserializeObject<Config>(json);
        }

    }
    public class Config
    {
        public string OutputPath =
            Path.Combine(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)), "GameLoader");
    }
}
