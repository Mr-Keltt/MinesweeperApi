namespace MinesweeperApi.Infrastructure.Data.Entities;

public class GameEntity
{
    public Guid Id { get; set; }
    public string BoardJson { get; set; }
}
