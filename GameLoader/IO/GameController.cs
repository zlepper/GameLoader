using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLoader;

namespace GameLoader.IO {
    
    public delegate void DoneEnablingGameEventHandler(Game game);
    public delegate void DoneDisablingGameEventHandler(Game game);

    public delegate void GameMoveProgressEventHandler(int progress, int count);

    public delegate void DoneMovingFilesEventHandler();
    /// <summary>
    /// Enables and disable games. 
    /// Also emits events when done
    /// </summary>
    public class GameController
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(
                  string lpFileName,
                  string lpExistingFileName,
                  IntPtr lpSecurityAttributes
              );

        private Game currentGame;

        public event DoneEnablingGameEventHandler DoneEnablingGame;
        
        public event DoneDisablingGameEventHandler DoneDisablingGame;

        public event GameMoveProgressEventHandler GameMoveProgress;

        public event DoneMovingFilesEventHandler DoneMovingFiles;

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
            GameMoveProgress?.Invoke(progress, count);
        }

        protected virtual void OnDoneMovingFiles()
        {
            DoneMovingFiles?.Invoke();
        }

        public void EnableGame(Game game)
        {
            switch (game.Status)
            {
                case GameStatus.Activated:
                    MessageBox.Show("Game is already activated, cannot activate again. ");
                    break;
                case GameStatus.Deactivated:
                    LoadGame(game);
                    break;
                case GameStatus.Loading:
                    MessageBox.Show("Game is currently loading. No point in doing it twice. ");
                    break;
                case GameStatus.Unloading:
                    MessageBox.Show(
                        "Game is currently unloading, please wait for it to finish before enabling it again. ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LoadGame(Game game)
        {
            currentGame = game;
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += BackgroundWorkerLoadGame;
                bw.RunWorkerAsync();
            }
        }

        private void BackgroundWorkerLoadGame(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var ldm = new LocalDataManager();
            var cfg = ldm.LoadConfig();
            if (!Directory.Exists(cfg.OutputPath))
            {
                Directory.CreateDirectory(cfg.OutputPath);
            }
            DirectoryInfo di = new DirectoryInfo(currentGame.Path);
            var files = di.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0, len = files.Length; i < len; i++)
            {
                OnGameMoveProgress(i, len);
                var file = files[i];
                file.CopyTo(CalculateOutputFilePath(cfg.OutputPath, file, di));
            }
            OnDoneMovingFiles();
            for(int i = 0, len = files.Length; i < len; i++)
            {
                OnGameMoveProgress(i, len);
                files[i].Delete();
            }
            di.Delete(true);

        }

        private string CalculateOutputFilePath(string outputDirectory, FileInfo file, DirectoryInfo inputDirectory)
        {
            string p = file.FullName.Replace(inputDirectory.FullName, "");
            return Path.Combine(outputDirectory, p);
        }

        public void DisableGame(Game game)
        {
        }
    }
}
