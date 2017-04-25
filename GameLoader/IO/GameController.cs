using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

    public delegate void FileMoveProgressEventHandler(long progress, long total);
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

        public event FileMoveProgressEventHandler FileMoveProgress;

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

        protected virtual void OnFileMovingProgress(long progress, long total)
        {
            FileMoveProgress?.Invoke(progress, total);
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

        public long GetTotalFreeSpace(string driveName)
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
                try
                {
                    //file.CopyTo(outputfile, true);
                    XCopy.Copy(file.FullName, outputfile, true, true, (o, pce) =>
                    {
                        OnFileMovingProgress(pce.Progress, pce.Total);
                    });
                }
                catch (Exception)
                {
                    if (!CalculateMd5(file.FullName).Equals(CalculateMd5(outputfile)))
                    {
                        MessageBox.Show(
                            $"Something went wrong when trying to move file \"{file.FullName}\" to \"{outputfile}\"");
                        return;
                    }
                }
            }
            OnDoneMovingFiles();
            string newPath = di.FullName + ".oldGameLoader";
            di.MoveTo(newPath);

            CreateDirectoryJunction(outputPath, currentGame.Path);
            OnDoneEnablingGame(currentGame);
        }

        public static string CalculateMd5(string file)
        {
            using (var md5 = MD5.Create())
            {
                while (true)
                {
                    try
                    {
                        using (var stream = File.OpenRead(file))
                        {
                            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                        }
                    }
                    catch
                    {
                        Thread.Sleep(100);
                    }
                }
            }
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

            DirectoryInfo gameDirectory = new DirectoryInfo(currentGame.Path + ".oldGameLoader");
            if (!gameDirectory.Exists)
            {
                gameDirectory = new DirectoryInfo(currentGame.Path);
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
            else
            {
                OnStartMovingFiles();
                if (Directory.Exists(currentGame.Path))
                {
                    Directory.Delete(currentGame.Path, true);
                }
                gameDirectory.MoveTo(currentGame.Path);

                OnDoneMovingFiles();

                DirectoryInfo gameLoaderDirectory = new DirectoryInfo(PathCombine(cfg.OutputPath, currentGame.Name));
                FileInfo[] files = gameLoaderDirectory.GetFiles("*", SearchOption.AllDirectories);
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
}
