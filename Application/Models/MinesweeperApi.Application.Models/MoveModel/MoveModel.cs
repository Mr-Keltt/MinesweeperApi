namespace MinesweeperApi.Application.Models;

public class MoveModel
{
    public Guid GameId { get; set; }
    public int Col { get; set; }
    public int Row { get; set; }
}
