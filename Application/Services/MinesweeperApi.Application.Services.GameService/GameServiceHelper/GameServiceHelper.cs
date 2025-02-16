namespace MinesweeperApi.Application.Services.GameService;

public static class GameServiceHelper
{
    public static int[,] InitializeField(int width, int height, int minesCount)
    {
        var field = new int[height, width];
        var random = new Random();
        int placedMines = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                field[i, j] = -2;
            }
        }

        while (placedMines < minesCount)
        {
            int x = random.Next(width);
            int y = random.Next(height);

            if (field[y, x] == 0)
            {
                field[y, x] = -1;
                placedMines++;
            }
        }
        return field;
    }
}
