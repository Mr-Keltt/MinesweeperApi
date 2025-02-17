namespace MinesweeperApi.Application.Models
{
    using System;

    /// <summary>
    /// Represents a model for making a move in the Minesweeper game.
    /// </summary>
    public class MoveModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the game in which the move is to be made.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Gets or sets the column index where the move is applied.
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Gets or sets the row index where the move is applied.
        /// </summary>
        public int Row { get; set; }
    }
}
