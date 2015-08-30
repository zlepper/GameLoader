using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLoader;

namespace GameLoader.IO
{

    public delegate void DoneEnablingGameEventHandler(Game game);
    public delegate void DoneDisablingGameEventHandler(Game game);

    public delegate void GameMoveProgressEventHandler(int progress, int count);

    public delegate void DoneMovingFilesEventHandler();

    public delegate void StartMovingFilesEventHandler();
    /// <summary>
    /// Enables and disable games. 
    /// Also emits events when done
    /// </summary>
    public class GameController
    {

        private Game currentGame;

        public event DoneEnablingGameEventHandler DoneEnablingGame;

        public event DoneDisablingGameEventHandler DoneDisablingGame;

        public event GameMoveProgressEventHandler GameMoveProgress;

        public event DoneMovingFilesEventHandler DoneMovingFiles;

        public event StartMovingFilesEventHandler StartMovingFiles;

        protected virtual void OnDoneEnablingGame(Game game)
        {
            DoneEnablingGame?.Invoke(game);
        }

        protected virtual void OnDoneDisablingGame(Game game)
        {
            DoneDisablingGame?.Invoke(game);
        }

        protected virtual void OnGameMoveProgress(int progress, int count)
        {
            GameMoveProgress?.Invoke(progress + 1, count);
        }

        protected virtual void OnDoneMovingFiles()
        {
            DoneMovingFiles?.Invoke();
        }

        protected virtual void OnStartMovingFiles()
        {
            StartMovingFiles?.Invoke();
        }

        public void EnableGame(Game game)
        {
            if (game.Status == GameStatus.Deactivated)
            {
                LoadGame(game);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(game.Status));
            }
        }

        private void LoadGame(Game game)
        {
            currentGame = game;
            if (HasEnoughSpaceOnFastDrive(game))
            {
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.DoWork += BackgroundWorkerLoadGame;
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("You do not have enough free space on your fast harddisk to load this game.");
            }
        }


        private bool HasEnoughSpaceOnFastDrive(Game game)
        {
            LocalDataManager ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            string letter = Path.GetPathRoot(cfg.OutputPath);
            return GetTotalFreeSpace(letter) > game.Size;
        }

        private long GetTotalFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.Name == driveName))
            {
                return drive.TotalFreeSpace;
            }
            return -1;
        }

        private void BackgroundWorkerLoadGame(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            currentGame.Status = GameStatus.Loading;
            LocalDataManager ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            string outputPath = Path.Combine(cfg.OutputPath, currentGame.Name);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            DirectoryInfo di = new DirectoryInfo(currentGame.Path);
            OnStartMovingFiles();
            FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories);
            currentGame.FileCount = files.Length;
            currentGame.Size = files.Sum(t => t.Length);
            for (int i = 0, len = files.Length; i < len; i++)
            {
                OnGameMoveProgress(i, len);
                FileInfo file = files[i];
                string outputfile = CalculateOutputFilePath(outputPath, file, di);
                FileInfo f = new FileInfo(outputfile);
                if (f.Directory != null && !f.Directory.Exists)
                {
                    f.Directory.Create();
                }
                file.CopyTo(outputfile, true);
            }
            OnDoneMovingFiles();
            for (int i = 0, len = files.Length; i < len; i++)
            {
                OnGameMoveProgress(i, len);
                files[i].Delete();
            }
            di.Delete(true);
            CreateDirectoryJunction(outputPath, currentGame.Path);
            OnDoneEnablingGame(currentGame);
        }

        private void CreateDirectoryJunction(string sourceDirectory, string destinationDirectory)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo("cmd.exe",
                "/c mklink " + $"/J \"{destinationDirectory}\" \"{sourceDirectory}\"")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process.Start(pInfo);
        }

        private string CalculateOutputFilePath(string outputDirectory, FileInfo file, DirectoryInfo inputDirectory)
        {
            string p = file.FullName.Replace(inputDirectory.FullName, "");
            string path = PathCombine(outputDirectory, p);
            return path;
        }

        private string CalculateOutputFilePath(DirectoryInfo outputDirectory, FileInfo file,
            DirectoryInfo inputDirectory)
        {
            return CalculateOutputFilePath(outputDirectory.FullName, file, inputDirectory);
        }

        private string PathCombine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }

        public void DisableGame(Game game)
        {
            if (game.Status == GameStatus.Activated)
            {
                UnloadGame(game);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(game.Status));
            }
        }

        private void UnloadGame(Game game)
        {
            currentGame = game;
            if (HasEnoughSpaceOnGameDrive(game))
            {
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.DoWork += BackgroundWorkerUnloadGame;
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("You do not have enough space on your gamedrive to unload the game!");
            }
        }

        private bool HasEnoughSpaceOnGameDrive(Game game)
        {
            string letter = Path.GetPathRoot(game.Path);
            return GetTotalFreeSpace(letter) > game.Size;
        }

        private void BackgroundWorkerUnloadGame(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            currentGame.Status = GameStatus.Unloading;
            LocalDataManager ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            DirectoryInfo gameDirectory = new DirectoryInfo(currentGame.Path);
            if (gameDirectory.Exists)
            {
                gameDirectory.Delete();
            }
            gameDirectory.Create();
            DirectoryInfo gameLoaderDirectory = new DirectoryInfo(PathCombine(cfg.OutputPath, currentGame.Name));
            OnStartMovingFiles();
            FileInfo[] files = gameLoaderDirectory.GetFiles("*", SearchOption.AllDirectories);
            currentGame.FileCount = files.Length;
            currentGame.Size = files.Sum(t => t.Length);
            for (int i = 0, len = files.Length; i < len; i++)
            {
                OnGameMoveProgress(i, len);
                FileInfo file = files[i];
                string outputfile = CalculateOutputFilePath(gameDirectory, file, gameLoaderDirectory);
                FileInfo f = new FileInfo(outputfile);
                if (f.Directory != null && !f.Directory.Exists)
                {
                    f.Directory.Create();
                }
                file.CopyTo(outputfile, true);
            }
            OnDoneMovingFiles();
            for (int i = 0, len = files.Length; i < len; i++)
            {
                OnGameMoveProgress(i, len);
                files[i].Delete();
            }
            gameLoaderDirectory.Delete(true);
            OnDoneDisablingGame(currentGame);
        }
    }
}
