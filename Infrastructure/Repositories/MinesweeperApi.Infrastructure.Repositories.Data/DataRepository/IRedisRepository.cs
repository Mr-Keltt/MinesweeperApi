namespace MinesweeperApi.Infrastructure.Repositories;

public interface IRedisRepository
{
    Task SetAsync(Guid key, int[,] data);
    Task<int[,]> GetAsync(Guid key);
    Task<bool> DeleteAsync(Guid key);
}


