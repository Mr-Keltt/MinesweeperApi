namespace MinesweeperApi.Application.Models;

public interface IGameModel
{
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Completed { get; set; }
    public int[,] CurrentField { get; set; }
}
