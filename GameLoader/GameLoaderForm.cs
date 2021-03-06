﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameLoader.IO;

namespace GameLoader
{
    public partial class GameLoaderForm : Form
    {
        /// <summary>
        /// We can't use a normal list, otherwise the datagridview will not update
        /// </summary>
        public BindingList<Game> Games;

        /// <summary>
        /// Indicates if we are already attempting to move a game.
        /// </summary>
        private bool movingGame = false;

        /// <summary>
        /// Keeps track of what we are currently doing
        /// </summary>
        private WorkingProgress workingProgress = WorkingProgress.BusyDoingNothing;

        public GameLoaderForm()
        {
            InitializeComponent();
            Closing += OnClosing;
            folderGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            folderGridView.UserDeletingRow += delegate(object sender, DataGridViewRowCancelEventArgs args)
            {
                Game game = args.Row.DataBoundItem as Game;
                if (game != null && game.Status != GameStatus.Deactivated)
                {
                    MessageBox.Show("You need to deactivate the game before you can delete it from GameLoader");
                    args.Cancel = true;
                }
                else
                {
                    LocalDataManager ldm = new LocalDataManager();
                    ldm.SaveGames(Games.ToList());
                }
            };
            SetupGamesView();
            LoadConfig();

            FileProgressStatusStripLabel.Text = "";
        }

        private void SetupGamesView()
        {
            Games = new BindingList<Game>(LoadData());
            BindingSource source = new BindingSource(Games, null);
            folderGridView.DataSource = source;
            foreach (DataGridViewColumn column in folderGridView.Columns)
            {
                switch (column.HeaderText)
                {
                    case "Size":
                        column.Visible = false;
                        break;
                    case "SizeForHumans":
                        column.HeaderText = "Size";
                        break;
                    case "FileCount":
                        column.HeaderText = "File Count";
                        break;
                }
            }
        }

        private void LoadConfig()
        {
            LocalDataManager ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            fastFolderTextBox.Text = cfg.OutputPath;
            if (cfg.FirstRun)
            {
                cfg.FirstRun = false;
                ldm.SaveConfig(cfg);
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.DoWork += BackgroundWorkerScanForGames;
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                SearchFoldersForGames(cfg.GamesFolders);
            }
        }

        private void BackgroundWorkerScanForGames(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            workingProgress = WorkingProgress.ScanningForGames;
            var ldm = new LocalDataManager();
            var cfg = ldm.LoadConfig();
            DialogResult result =
                    MessageBox.Show(
                        "This is the first time you are running GameLoader, do you want it to check for installed games?",
                        "Check for games", MessageBoxButtons.YesNo);
            List<string> fs = cfg.GamesFolders;
            if (result == DialogResult.Yes)
            {
                string[] paths = GameSuggestions.GetGameFolders();
                string question =
                    $"I found these paths: {Environment.NewLine}{string.Join(Environment.NewLine, paths)}{Environment.NewLine}Do you want to use them?{Environment.NewLine}You can add other folders to auto-discovery later if you want. ";
                result = MessageBox.Show(question, "Found paths", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    fs.AddRange(paths.Distinct());
                }
            }
            ldm.SaveConfig(cfg);
            SearchFoldersForGames(fs);
        }

        private void SearchFoldersForGames(List<string> fs)
        {
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += delegate
                {
                    foreach (string folder in fs)
                    {
                        string[] paths = GameSuggestions.GetGameFolders(folder);
                        GameAdder ga = new GameAdder();
                        ga.DataReady += AdderOnDataReady;
                        ga.AddGames(paths);
                    }
                    workingProgress = WorkingProgress.BusyDoingNothing;
                };
                bw.RunWorkerAsync();
            }
        }

        private void AdderOnDataReady(string path, long size, int count, string name)
        {
            if (folderGridView.InvokeRequired)
            {
                folderGridView.BeginInvoke(new Action(() => AdderOnDataReady(path, size, count, name)));
            }
            else
            {
                Game game = new Game(path, name, size, count);
                // Don't add a game already in the list
                if (Games.Any(g => g.Path.Equals(game.Path))) return;
                if (game.Path.EndsWith(".oldGameLoader")) return;
                Games.Add(game);
                SaveData();
                folderGridView.Refresh();
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            // We really shouldn't stop the process in the middle of moving files
            if (workingProgress == WorkingProgress.BusyDoingNothing)
            {
                SaveData();
                return;
            }
            DialogResult r = MessageBox.Show("Currently moving a game(Or scanning for games), Closing the window now will cause issues, do you still want to exit GameLoader?", "Moving files!", MessageBoxButtons.YesNo);
            if (r == DialogResult.No)
            {
                cancelEventArgs.Cancel = true;
            }
        }

        private void SaveData()
        {
            List<Game> ga = Games.ToList();
            LocalDataManager ldm = new LocalDataManager();
            ldm.SaveGames(ga);
        }

        private List<Game> LoadData()
        {
            LocalDataManager ldm = new LocalDataManager();
            return ldm.LoadGames();

        }

        private void addNewGameButton_Click(object sender, EventArgs e)
        {
            // Get the user input
            string path = newGamePathTextBox.Text;
            string name = newGameName.Text;
            if (string.IsNullOrWhiteSpace(name)) return;

            // Make sure the user is not adding a game already in the list
            if (Games.Any(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || g.Path.Equals(path)))
            {
                MessageBox.Show("This game has already been added to the library");
                return;
            }
            
            IOChecker checker = new IOChecker();
            DirectoryInfo result = checker.ShouldUseDir(path);
            if (result == null) return;
            
            GameAdder adder = new GameAdder();
            adder.DataReady += AdderOnDataReady;
            adder.AddGame(result, name);
            newGamePathTextBox.Text = "";
            newGameName.Text = "";
        }

        private void folderGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridViewRow currentrow = folderGridView.CurrentRow;
            Game game = currentrow?.DataBoundItem as Game;
            if (game == null)
            {
                gameDataGroupbox.Enabled = false;
            }
            else
            {
                gameDataGroupbox.Enabled = true;
                nameEditTextBox.Text = game.Name;
                pathEditTextBox.Text = game.Path;
                activateGameButton.Text = game.Status == GameStatus.Activated ? "Deactivate" : "Activate";
            }
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentrow = folderGridView.CurrentRow;
            Game game = currentrow?.DataBoundItem as Game;
            if (game == null) return;
            if (game.Status == GameStatus.Deactivated)
            {
                game.Name = nameEditTextBox.Text;
                game.Path = pathEditTextBox.Text;
                SaveData();
                folderGridView.Refresh();
            }
            else
            {
                MessageBox.Show("Game is currently " + game.Status + ", and therefor cannot be edited." + Environment.NewLine + (game.Status == GameStatus.Unloading
                    ? "Please wait for the game to unload." : "Please unload the game first."));
            }
        }

        private void activateGameButton_Click(object sender, EventArgs e)
        {
            if (movingGame)
            {
                MessageBox.Show("Already moving a game, please wait for it to finish. ");
            }
            else
            {
                // Get the selected game to activate
                DataGridViewRow currentrow = folderGridView.CurrentRow;
                Game game = currentrow?.DataBoundItem as Game;
                if (game == null)
                {
                    MessageBox.Show("Please select a game first");
                    return;
                }
                GameController gameController;
                switch (game.Status)
                {
                    case GameStatus.Deactivated:
                        gameController = new GameController();
                        gameController.StartMovingFiles += GameControllerOnStartMovingFiles;
                        gameController.DoneMovingFiles += GameControllerOnDoneMovingFiles;
                        gameController.GameMoveProgress += GameControllerOnGameMoveProgress;
                        gameController.DoneEnablingGame += GameControllerOnDoneEnablingGame;
                        gameController.FileMoveProgress += GameControllerOnFileMoveProgress;
                        gameController.EnableGame(game);
                        break;
                    case GameStatus.Activated:
                        gameController = new GameController();
                        gameController.StartMovingFiles += GameControllerOnStartMovingFiles;
                        gameController.DoneMovingFiles += GameControllerOnDoneMovingFiles;
                        gameController.GameMoveProgress += GameControllerOnGameMoveProgress;
                        gameController.DoneDisablingGame += GameControllerOnDoneDisablingGame;
                        gameController.DisableGame(game);
                        break;
                    case GameStatus.Loading:
                        MessageBox.Show("Game is currently being loaded, please wait for it to finish. ");
                        break;
                    case GameStatus.Unloading:
                        MessageBox.Show("Game is currently being unloaded, please wait for it to finish");
                        break;
                    default:
                        MessageBox.Show("DUCK!! Please show this to a developer, because something broke!" + Environment.NewLine + Environment.StackTrace);
                        break;
                }
            }
        }

        private void GameControllerOnFileMoveProgress(long progress, long total)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new Action(() => GameControllerOnFileMoveProgress(progress, total)));
                return;
            }
            string progressText = progress.ToString().PadRight(total.ToString().Length);

            FileProgressStatusStripLabel.Text = $"{progressText} of {total} bytes copied";
        }

        private void GameControllerOnDoneDisablingGame(Game game)
        {
            if (folderGridView.InvokeRequired)
            {
                folderGridView.Invoke(new Action(() => GameControllerOnDoneDisablingGame(game)));
            }
            else
            {
                game.Status = GameStatus.Deactivated;
                workingProgress = WorkingProgress.BusyDoingNothing;
                folderGridView.Refresh();
                statusToolStripLabel.Text = "Ready!";
                SaveData();
                folderGridView_CurrentCellChanged(null, null);
            }
        }

        private void GameControllerOnStartMovingFiles()
        {
            if (folderGridView.InvokeRequired)
            {
                folderGridView.Invoke(new Action(() => folderGridView.Refresh()));
            }
            workingProgress = WorkingProgress.Moving;
        }

        private void GameControllerOnDoneEnablingGame(Game game)
        {
            if (folderGridView.InvokeRequired)
            {
                folderGridView.Invoke(new Action(() => GameControllerOnDoneEnablingGame(game)));
            }
            else
            {
                game.Status = GameStatus.Activated;
                workingProgress = WorkingProgress.BusyDoingNothing;
                folderGridView.Refresh();
                statusToolStripLabel.Text = "Ready!";
                SaveData();
                folderGridView_CurrentCellChanged(null, null);
                FileProgressStatusStripLabel.Text = "";
            }
        }

        private void GameControllerOnGameMoveProgress(int progress, int count)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new Action(() => GameControllerOnGameMoveProgress(progress, count)));
            }
            else
            {
                switch (workingProgress)
                {
                    case WorkingProgress.Moving:
                        statusToolStripLabel.Text = $"Copying file {progress} of {count}!";
                        break;
                    case WorkingProgress.Deleting:
                        statusToolStripLabel.Text = $"Deleting file {progress} of {count}!";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (statusStipProgressBar.ProgressBar != null)
                {
                    statusStipProgressBar.ProgressBar.Maximum = count;
                    statusStipProgressBar.ProgressBar.Value = progress;
                }
            }
        }

        private void GameControllerOnDoneMovingFiles()
        {
            workingProgress = WorkingProgress.Deleting;
        }

        private enum WorkingProgress
        {
            Moving,
            Deleting,
            BusyDoingNothing,
            ScanningForGames
        }

        private void saveFastFolderButton_Click(object sender, EventArgs e)
        {
            if (Games.Any(g => g.Status != GameStatus.Deactivated))
            {
                MessageBox.Show("Not all games are deactivated, please do this first!");
                return;
            }
            string text = fastFolderTextBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("You have to enter a path");
                return;
            }
            if (File.Exists(text))
            {
                MessageBox.Show("There is a file at this path. It is therefor not valid");
                return;
            }
            if (!Directory.Exists(text))
            {
                DialogResult result = MessageBox.Show("The destination folder does not exist, do you want me to create it?", "Directory not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    Directory.CreateDirectory(text);
                else
                    return;
            }
            LocalDataManager ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            cfg.OutputPath = text;
            ldm.SaveConfig(cfg);
        }

        private void newGamePathTextBox_TextChanged(object sender, EventArgs e)
        {
            string t = newGamePathTextBox.Text;
            if (string.IsNullOrWhiteSpace(t)) return;
            if (!string.IsNullOrWhiteSpace(newGameName.Text)) return;
            DirectoryInfo f = new DirectoryInfo(t);
            if (!f.Exists) return;
            newGameName.Text = f.Name;
        }

        private void AddAutodiscoveryFolderButton_Click(object sender, EventArgs e)
        {
            string t = AddAutoDiscoveryTextBox.Text;
            if (string.IsNullOrWhiteSpace(t)) return;
            if (!Directory.Exists(t))
            {
                MessageBox.Show("Directory does not exist");
                return;
            }
            LocalDataManager ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            cfg.GamesFolders.Add(t);
            GameAdder ga = new GameAdder();
            ga.DataReady += AdderOnDataReady;
            ga.AddGames(GameSuggestions.GetGameFolders(t));
            AddAutoDiscoveryTextBox.Text = "";
        }

        private void BrowseForGamePathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "Select the root folder of the game install.",
                ShowNewFolderButton = false
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Cannot find directory, please try again.");
                    return;
                }
                newGamePathTextBox.Text = path;
            }
        }

        private void BrowseForAutoDiscoveryFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "Select a folder which contains a lot of game.",
                ShowNewFolderButton = false
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Cannot find directory, please try again.");
                    return;
                }
                AddAutoDiscoveryTextBox.Text = path;
            }
        }

        private void resetApplicationStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to reset the application state? If you have any games currently loaded, you will have to reset them manually. ", "Reset application state", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                LocalDataManager.ResetApplication();
                SetupGamesView();
                LoadConfig();
            }
        }

        private void fastFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = fastFolderTextBox.Text;
            var gameController = new GameController();
            if (Path.IsPathRooted(text))
            {
                var totalFreeSpace = gameController.GetTotalFreeSpace(Path.GetPathRoot(text));
                var bytes = Formatters.BytesToReadable(totalFreeSpace);
                DiskSpaceLeftLabel.Text = bytes;
            }
            else
            {
                DiskSpaceLeftLabel.Text = "";
            }
        }
    }
}
