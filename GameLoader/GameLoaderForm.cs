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

        public GameLoaderForm()
        {
            InitializeComponent();
            this.Closing += OnClosing;
            Games = new BindingList<Game>(LoadData());
            var source = new BindingSource(Games, null);
            folderGridView.DataSource = source;
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
            if (game != null)
            {
                Debug.WriteLine(game.Name);
                nameEditTextBox.Text = game.Name;
                pathEditTextBox.Text = game.Path;
            }
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {

        }
    }
}
