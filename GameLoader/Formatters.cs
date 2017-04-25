using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLoader
{
    public class Formatters
    {
        private static string[] sizes = {"B", "KB", "MB", "GB", "TB"};

        public static string BytesToReadable(long bytes)
        {
            int index = 0;
            while (bytes > 10000)
            {
                bytes /= 1000;
                index++;
            }
            return $"{bytes}{sizes[index]}";
        }
    }
}
