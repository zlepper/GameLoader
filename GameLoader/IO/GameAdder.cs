using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLoader.IO
{
    public delegate void DataReadyEventHandler(string path, long size, int count, string name);

    public class GameAdder
    {
        private DirectoryInfo directory;
        private string name;

        public event DataReadyEventHandler DataReady;

        protected virtual void OnDataReady(string path, long size, int count, string name)
        {
            DataReady?.Invoke(path, size, count, name);
        }

        public void AddGame(DirectoryInfo d, string name)
        {
            directory = d;
            this.name = name;
            // Move the data getting work into the background
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += GetFileData;
                bw.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Count the number of files and the total size of the directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="doWorkEventArgs"></param>
        private void GetFileData(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            long totalFilesSize = 0;
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            int count = files.Length;
            // Get the total size of the directory
            for (int i = 0; i < count; i++)
            {
                totalFilesSize += files[i].Length;
            }

            // Emit an event we can listen for
            OnDataReady(directory.FullName, totalFilesSize, count, name);
        }
    }
}
