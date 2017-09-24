namespace GameLoader
{
    public class Game
    {
        private const long KB = 1000L;
        private const long MB = 1000 * 1000L;
        private const long GB = 1000 * 1000 * 1000L;
        private const long TB = 1000 * 1000 * 1000 * 1000L;

        /// <summary>
        /// The path to the game
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The name of the game
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The current status of the game
        /// </summary>
        public GameStatus Status { get; set; }

        /// <summary>
        /// The total size of the game
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The size of the game, in a readable format
        /// </summary>
        public string SizeForHumans
        {
            get
            {
                if (Size < KB)
                {
                    return Size + " B";
                }
                if (Size < MB)
                {
                    return Size / KB + " KB";
                }
                if (Size < GB)
                {
                    return Size / MB + " MB";
                }
                if (Size < TB)
                {
                    return Size / GB + " GB";
                }
                // Probably not needed, but somewhere, someone is going to need this...
                return Size / TB + " TB";
            }
        }

        /// <summary>
        /// The number of files in the game
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="path">The path to the game</param>
        /// <param name="name">The name of the game</param>
        /// <param name="fileCount">The number of files in the game</param>
        /// <param name="status">The current status of the game</param>
        /// <param name="size">The total size of the game</param>
        public Game(string path, string name, long size, int fileCount, GameStatus status = GameStatus.Deactivated)
        {
            Path = path;
            Name = name;
            Size = size;
            FileCount = fileCount;
            Status = status;
        }
    }
}