namespace GameLoader
{
    public class Game
    {
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