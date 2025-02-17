using System;
using System.Collections.Generic;

namespace MinesweeperApi.Application.Services.GameService
{
    /// <summary>
    /// Provides helper methods for initializing and manipulating the game field for Minesweeper.
    /// </summary>
    public static class GameServiceHelper
    {
        /// <summary>
        /// Initializes an empty game field with the specified dimensions.
        /// Each cell is set to -1 indicating an unrevealed cell.
        /// </summary>
        /// <param name="width">The width (number of columns) of the field.</param>
        /// <param name="height">The height (number of rows) of the field.</param>
        /// <returns>A two-dimensional array representing the empty game field.</returns>
        public static int[,] InitializeEmptyField(int width, int height)
        {
            var field = new int[height, width];

            // Set each cell to -1 to denote that it is unrevealed.
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    field[i, j] = -1;

            return field;
        }

        /// <summary>
        /// Randomly places the specified number of bombs (mines) on the game field,
        /// ensuring that the first move's cell is not occupied by a bomb.
        /// Bomb cells are marked with -2.
        /// </summary>
        /// <param name="field">The game field to place bombs in.</param>
        /// <param name="minesCount">The total number of bombs to place.</param>
        /// <param name="firstRow">The row index of the first move (to be excluded from bomb placement).</param>
        /// <param name="firstCol">The column index of the first move (to be excluded from bomb placement).</param>
        /// <returns>The updated game field with bombs placed.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the number of mines exceeds the maximum allowed (total cells minus one).
        /// </exception>
        public static int[,] PlaceBombs(int[,] field, int minesCount, int firstRow, int firstCol)
        {
            int height = field.GetLength(0);
            int width = field.GetLength(1);

            if (minesCount > (width * height - 1))
                throw new ArgumentException($"Количество мин должно быть не больше {width * height - 1}.");

            var random = new Random();
            int placedMines = 0;

            // Continue placing bombs until the required count is reached.
            while (placedMines < minesCount)
            {
                int x = random.Next(width);
                int y = random.Next(height);

                // Skip if the randomly selected cell is the first move or already has a bomb.
                if ((y == firstRow && x == firstCol) || field[y, x] == -2)
                    continue;

                // Mark the cell with a bomb indicator (-2).
                field[y, x] = -2;
                placedMines++;
            }

            return field;
        }

        /// <summary>
        /// Determines whether the game field is in its initial empty state.
        /// A field is considered empty if every cell is set to -1.
        /// </summary>
        /// <param name="field">The game field to check.</param>
        /// <returns>
        /// <c>true</c> if all cells in the field are -1; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFieldEmpty(int[,] field)
        {
            int height = field.GetLength(0);
            int width = field.GetLength(1);

            // Check each cell; if any cell is not -1, the field is not empty.
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (field[i, j] != -1)
                        return false;

            return true;
        }

        /// <summary>
        /// Reveals the game field based on the player's move at the specified cell.
        /// If a bomb is revealed, the entire field is updated accordingly.
        /// Otherwise, the method reveals contiguous cells based on the Minesweeper rules.
        /// </summary>
        /// <param name="row">The row index of the cell to reveal.</param>
        /// <param name="col">The column index of the cell to reveal.</param>
        /// <param name="currentField">The current state of the game field.</param>
        /// <returns>The updated game field after revealing cells.</returns>
        public static int[,] RevealField(int row, int col, int[,] currentField)
        {
            int numRows = currentField.GetLength(0);
            int numCols = currentField.GetLength(1);

            // If the cell is already revealed (values between 0 and 8), return the current field.
            if (currentField[row, col] >= 0 && currentField[row, col] <= 8)
                return currentField;

            // If a bomb is revealed, mark all bombs as -3 and count adjacent mines for unrevealed cells.
            if (currentField[row, col] == -2)
            {
                for (int r = 0; r < numRows; r++)
                {
                    for (int c = 0; c < numCols; c++)
                    {
                        if (currentField[r, c] == -2)
                            currentField[r, c] = -3;
                        else if (currentField[r, c] == -1)
                            currentField[r, c] = CountAdjacentMines(currentField, r, c);
                    }
                }
                return currentField;
            }

            // Use a queue for breadth-first search to reveal contiguous cells.
            Queue<(int row, int col)> queue = new Queue<(int, int)>();
            queue.Enqueue((row, col));

            while (queue.Count > 0)
            {
                var (r, c) = queue.Dequeue();

                // Skip cells that are already revealed.
                if (currentField[r, c] != -1)
                    continue;

                // Count the number of adjacent bombs.
                int adjacentMines = CountAdjacentMines(currentField, r, c);
                currentField[r, c] = adjacentMines;

                // If there are no adjacent bombs, add neighboring cells to the queue.
                if (adjacentMines == 0)
                {
                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            if (dr == 0 && dc == 0)
                                continue;

                            int newRow = r + dr, newCol = c + dc;

                            // Ensure the new cell is within bounds and unrevealed.
                            if (newRow >= 0 && newRow < numRows && newCol >= 0 && newCol < numCols)
                                if (currentField[newRow, newCol] == -1)
                                    queue.Enqueue((newRow, newCol));
                        }
                    }
                }
            }

            // If the player wins (all non-bomb cells are revealed), mark remaining bombs with a win indicator (-4).
            if (IsWin(currentField))
            {
                for (int r = 0; r < numRows; r++)
                {
                    for (int c = 0; c < numCols; c++)
                    {
                        if (currentField[r, c] == -2)
                            currentField[r, c] = -4;
                    }
                }
            }

            return currentField;
        }

        /// <summary>
        /// Counts the number of bombs adjacent to the specified cell in the game field.
        /// </summary>
        /// <param name="field">The game field.</param>
        /// <param name="r">The row index of the cell.</param>
        /// <param name="c">The column index of the cell.</param>
        /// <returns>The count of adjacent bombs.</returns>
        private static int CountAdjacentMines(int[,] field, int r, int c)
        {
            int count = 0;
            int numRows = field.GetLength(0);
            int numCols = field.GetLength(1);

            // Check all adjacent cells (including diagonals).
            for (int dr = -1; dr <= 1; dr++)
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                        continue;

                    int newRow = r + dr, newCol = c + dc;

                    // If the adjacent cell is within bounds and contains a bomb, increment the count.
                    if (newRow >= 0 && newRow < numRows && newCol >= 0 && newCol < numCols)
                        if (field[newRow, newCol] == -2)
                            count++;
                }

            return count;
        }

        /// <summary>
        /// Determines whether the game has been won.
        /// The game is considered won if there are no unrevealed cells (-1) remaining.
        /// </summary>
        /// <param name="field">The current state of the game field.</param>
        /// <returns>
        /// <c>true</c> if the game is won; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWin(int[,] field)
        {
            int numRows = field.GetLength(0);
            int numCols = field.GetLength(1);

            // If any cell is still unrevealed (-1), the game is not won.
            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < numCols; j++)
                    if (field[i, j] == -1)
                        return false;

            return true;
        }
    }
}
