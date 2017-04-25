using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLoader.IO;

namespace GameLoader
{
    public static class GameSuggestions
    {
        /// <summary>
        /// Gets possible game locations
        /// </summary>
        /// <returns>A list of possible game locations</returns>
        public static string[] GetGameFolders()
        {
            return FindOtherGameFolders();
        }

        /// <summary>
        /// Checks all the drives on the computer for game locations
        /// </summary>
        /// <returns></returns>
        private static string[] FindOtherGameFolders()
        {
            var libraryLocations = new List<string>();
            // Iterate over all the drives on the computer
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                // Get the folders in the root of the drive
                var driveDirectory = new DirectoryInfo(drive.Name);
                // Scan the drive recursive, up to 4 levels deep
                // Any deeper than this and this will take forever
                ScanRecursive(0, 4, libraryLocations, driveDirectory);
            }
            return libraryLocations.ToArray();
        }

        /// <summary>
        /// Check a folder for valid game folders, and adds them to the list
        /// And if not at the max level yet, scans sub folders
        /// </summary>
        /// <param name="current">How deep we currently are</param>
        /// <param name="max">How deep we can go max</param>
        /// <param name="libraryLocations">A list of all the current game locations</param>
        /// <param name="folder">The folder to scan</param>
        private static void ScanRecursive(int current, int max, List<string> libraryLocations, DirectoryInfo folder)
        {
            // Make sure we are not too deep
            if (max >= current)
            {
                // Get all the sub folders and iterate over them
                DirectoryInfo[] folders;
                try
                {
                    folders = folder.GetDirectories();
                }
                    // Make sure we have access to read the folder
                catch (UnauthorizedAccessException)
                {
                    return;
                }
                // Avoid any other errors
                catch (IOException)
                {
                    return;
                }
               
                foreach (DirectoryInfo f in folders)
                {
                    // Check for special windows directories
                    if (f.Name.StartsWith("$"))
                    {
                        continue;
                    }
                    // Check if the directory is a hidden directory
                    // because we shouldn't search those
                    if ((f.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        continue;
                    }
                    // Another data directory which we should skip
                    if (f.Name.Equals("Common Files"))
                    {
                        continue;
                    }
                    // Check if we can a match on the folder
                    if (!CheckFolder(f, libraryLocations))
                    {
                        // We didn't, so we should scan the subfolders
                        ScanRecursive(current + 1, max, libraryLocations, f);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a folder match any know game library names
        /// </summary>
        /// <param name="folder">The folder to check</param>
        /// <param name="libraryLocations">A list to add the folder to if it's valid</param>
        /// <returns></returns>
        private static bool CheckFolder(DirectoryInfo folder, List<string> libraryLocations)
        {
            // Check for steam libraries
            if (folder.FullName.EndsWith("SteamLibrary\\steamapps\\common", StringComparison.OrdinalIgnoreCase))
            {
                libraryLocations.Add(folder.FullName);
                return true;
            }
            // Check for origin libraries
            if (folder.Name.Equals("Origin Games", StringComparison.OrdinalIgnoreCase))
            {
                libraryLocations.Add(folder.FullName);
                return true;
            }
            // Check for the steam install itself
            if (folder.Name.Equals("Steam", StringComparison.OrdinalIgnoreCase))
            {
                // Make sure to get the correct sub folder
                libraryLocations.Add(FindSteamCommonsFolder(folder));
                return true;
            }
            // Check for a ubisoft install
            if (folder.Name.Equals("Ubisoft", StringComparison.OrdinalIgnoreCase))
            {
                // Make sure to get the correct subfolder
                libraryLocations.Add(FindUbisoftFolder(folder));
                return true;
            }
            // Check for a GOG client install
            if (folder.Name.Equals("GalaxyClient", StringComparison.OrdinalIgnoreCase))
            {
                // Make sure to get the correct sub folder
                libraryLocations.Add(FindGalaxyClientFolder(folder));
                return true;
            }
            // Check for any directories called "games" since they often contain games
            if (folder.Name.Equals("Games", StringComparison.OrdinalIgnoreCase))
            {
                libraryLocations.Add(folder.FullName);
                return true;
            }
            // The folder didn't match a know pattern, so we should search the sub folders
            return false;
        }

        /// <summary>
        /// Special formatter for the GOG client
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static string FindGalaxyClientFolder(DirectoryInfo folder)
        {
            return PathCombine(folder.FullName, "Games");
        }

        /// <summary>
        /// Special formatter for the ubisoft client
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static string FindUbisoftFolder(DirectoryInfo folder)
        {
            return PathCombine(PathCombine(folder.FullName, "Ubisoft Game Launcher"), "games");
        }

        /// <summary>
        /// Special formatter for the Steam client
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static string FindSteamCommonsFolder(DirectoryInfo folder)
        {
            return PathCombine(PathCombine(folder.FullName, "steamapps"), "common");
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
