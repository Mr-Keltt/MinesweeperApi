using System;
using System.Collections.Generic;

namespace MinesweeperApi.Application.Services.GameService
{
    public static class GameServiceHelper
    {
        public static int[,] InitializeEmptyField(int width, int height)
        {
            var field = new int[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    field[i, j] = -1;
            return field;
        }

        public static int[,] PlaceBombs(int[,] field, int minesCount, int firstRow, int firstCol)
        {
            int height = field.GetLength(0);
            int width = field.GetLength(1);

            if (minesCount > (width * height - 1))
                throw new ArgumentException($"Количество мин должно быть не больше {width * height - 1}.");

            var random = new Random();
            int placedMines = 0;
            while (placedMines < minesCount)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                if ((y == firstRow && x == firstCol) || field[y, x] == -2)
                    continue;
                field[y, x] = -2;
                placedMines++;
            }
            return field;
        }

        public static bool IsFieldEmpty(int[,] field)
        {
            int height = field.GetLength(0);
            int width = field.GetLength(1);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (field[i, j] != -1)
                        return false;
            return true;
        }

        public static int[,] RevealField(int row, int col, int[,] currentField)
        {
            int numRows = currentField.GetLength(0);
            int numCols = currentField.GetLength(1);

            if (currentField[row, col] >= 0 && currentField[row, col] <= 8)
                return currentField;

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

            Queue<(int row, int col)> queue = new Queue<(int, int)>();
            queue.Enqueue((row, col));
            while (queue.Count > 0)
            {
                var (r, c) = queue.Dequeue();
                if (currentField[r, c] != -1)
                    continue;
                int adjacentMines = CountAdjacentMines(currentField, r, c);
                currentField[r, c] = adjacentMines;
                if (adjacentMines == 0)
                {
                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            if (dr == 0 && dc == 0)
                                continue;
                            int newRow = r + dr, newCol = c + dc;
                            if (newRow >= 0 && newRow < numRows && newCol >= 0 && newCol < numCols)
                                if (currentField[newRow, newCol] == -1)
                                    queue.Enqueue((newRow, newCol));
                        }
                    }
                }
            }

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

        private static int CountAdjacentMines(int[,] field, int r, int c)
        {
            int count = 0;
            int numRows = field.GetLength(0);
            int numCols = field.GetLength(1);
            for (int dr = -1; dr <= 1; dr++)
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                        continue;
                    int newRow = r + dr, newCol = c + dc;
                    if (newRow >= 0 && newRow < numRows && newCol >= 0 && newCol < numCols)
                        if (field[newRow, newCol] == -2)
                            count++;
                }
            return count;
        }

        public static bool IsWin(int[,] field)
        {
            int numRows = field.GetLength(0);
            int numCols = field.GetLength(1);
            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < numCols; j++)
                    if (field[i, j] == -1)
                        return false;
            return true;
        }
    }
}
