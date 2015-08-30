using System;
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
        // We can't use a normal list, otherwise the datagridview will not update
        public BindingList<Game> Games;

        /// <summary>
        /// Indicates if we are already attempting to move a game.
        /// </summary>
        private bool movingGame = false;

        private WorkingProgress workingProgress = WorkingProgress.BusyDoingNothing;

        public GameLoaderForm()
        {
            InitializeComponent();
            this.Closing += OnClosing;
            Games = new BindingList<Game>(LoadData());
            var source = new BindingSource(Games, null);
            folderGridView.DataSource = source;
            LoadConfig();
        }

        private void LoadConfig()
        {
            var ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            fastFolderTextBox.Text = cfg.OutputPath;

        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            SaveData();
        }

        private void SaveData()
        {
            List<Game> ga = Games.ToList();
            var ldm = new LocalDataManager();
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
            adder.DataReady += (path1, size, fileCount, name1) => AdderOnDataReady(path1, name1, size, fileCount);
            adder.AddGame(result, name);
        }

        private void AdderOnDataReady(string path, string name, long size, int fileCount)
        {
            if (folderGridView.InvokeRequired)
            {
                folderGridView.BeginInvoke(new Action(() => AdderOnDataReady(path, name, size, fileCount)));
            }
            else
            {
                Game game = new Game(path, name, size, fileCount);
                Games.Add(game);
                SaveData();
            }
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
                game.Status = GameStatus.Loading;
                folderGridView.Refresh();
                GameController gameController = new GameController();
                gameController.DoneMovingFiles += GameControllerOnDoneMovingFiles;
                gameController.GameMoveProgress += GameControllerOnGameMoveProgress;
                gameController.DoneEnablingGame += GameControllerOnDoneEnablingGame;
                gameController.EnableGame(game);
            }
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
                        statusToolStripLabel.Text = string.Format("Moving file {0} of {1}!", progress, count);
                        break;
                    case WorkingProgress.Deleting:
                        statusToolStripLabel.Text = string.Format("Deleting file {0} of {1}!", progress, count);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
            BusyDoingNothing
        }

        private void saveFastFolderButton_Click(object sender, EventArgs e)
        {
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
            var ldm = new LocalDataManager();
            Config cfg = ldm.LoadConfig();
            cfg.OutputPath = text;
            ldm.SaveConfig(cfg);
        }
    }
}
