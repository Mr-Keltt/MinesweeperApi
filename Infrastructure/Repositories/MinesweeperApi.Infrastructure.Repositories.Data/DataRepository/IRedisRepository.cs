using MinesweeperApi.Infrastructure.Data.Entities;

namespace MinesweeperApi.Infrastructure.Repositories;

public interface IRedisRepository
{
    Task<GameEntity> SetAsync(GameEntity newGame);
    Task<GameEntity> GetAsync(Guid gameId);
    Task DeleteAsync(Guid id);
}


