using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLoader.IO
{
    public class IOChecker
    {
        public DirectoryInfo ShouldUseDir(string path)
        {
            // Make sure the user entered something
            if (string.IsNullOrWhiteSpace(path)) return null;


            // Make sure whatever the user entered actually exists
            if (!FileOrDirectoryExists(path))
            {
                
                MessageBox.Show("Path does not exist. ", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            FileInfo file = new FileInfo(path);

            // Check that the user entered a directory
            FileAttributes attr = file.Attributes;
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // It's a directory
                return new DirectoryInfo(file.FullName);
            }
            // It's a file
            // Ask the user if we should use the parent directory instead.
            DialogResult dr =
                MessageBox.Show(
                    $"The path you entered is to a file. Do you want to use {file.Directory} instead?",
                    "It's a file!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return dr == DialogResult.Yes ? file.Directory : null;
        }

        public static bool FileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) || File.Exists(name));
        }
    }
}
