namespace MinesweeperApi.Application.Models;

public interface MoveModel : IMoveModel
{
    Guid GameId { get; set; }
    int Col { get; set; }
    int Row { get; set; }
}
