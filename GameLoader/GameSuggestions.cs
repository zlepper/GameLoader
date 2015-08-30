using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLoader.IO;

namespace GameLoader
{
    public class GameSuggestions
    {
        public static readonly string[] DefaultGameLocations = new []
        {
            @"Program Files (x86)\Origin Games",
            @"Program Files (x86)\Steam\steamapps\common",
            @"Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games",
            @"Program Files (x86)\GalaxyClient\Games"
        };

        public static string[] GetGameFolders()
        {
            return (DriveInfo.GetDrives()
                .SelectMany(driveInfo => DefaultGameLocations,
                    (driveInfo, defaultGameLocation) => PathCombine(driveInfo.Name, defaultGameLocation))
                .Where(Directory.Exists)).ToArray();
        }

        public static string[] GetGameFolders(DirectoryInfo dir)
        {
            if (dir.Exists)
            {
                return dir.GetDirectories().Select(m => m.FullName).ToArray();
            } 
            return new string[0];
        }

        public static string[] GetGameFolders(string dir)
        {
            return GetGameFolders(new DirectoryInfo(dir));
        }


        private static string PathCombine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }

    }
}
