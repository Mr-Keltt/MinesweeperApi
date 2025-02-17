namespace MinesweeperApi.Application.Models
{
    /// <summary>
    /// Defines the contract for a game model containing configuration and state properties for a game.
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// Gets or sets the width of the game board.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game board.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is completed.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the current state of the game board represented as a two-dimensional array.
        /// </summary>
        public int[,] CurrentField { get; set; }
    }
}
