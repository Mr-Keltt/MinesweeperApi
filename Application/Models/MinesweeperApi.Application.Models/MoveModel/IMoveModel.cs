namespace MinesweeperApi.Application.Models;

public interface IMoveModel
{
    Guid GameId { get; set; }
    int Col {  get; set; }
    int Row { get; set; }
}
