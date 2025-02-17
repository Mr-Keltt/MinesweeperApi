using MinesweeperApi.Infrastructure.Data.Entities;
using System;
using System.Threading.Tasks;

namespace MinesweeperApi.Infrastructure.Repositories
{
    /// <summary>
    /// Defines methods for performing CRUD operations on GameEntity objects using a Redis data store.
    /// </summary>
    public interface IRedisRepository
    {
        /// <summary>
        /// Asynchronously creates or updates a game entity in the Redis data store.
        /// </summary>
        /// <param name="newGame">The game entity to create or update.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the created or updated <see cref="GameEntity"/>.
        /// </returns>
        Task<GameEntity> SetAsync(GameEntity newGame);

        /// <summary>
        /// Asynchronously retrieves a game entity by its unique identifier from the Redis data store.
        /// </summary>
        /// <param name="gameId">The unique identifier of the game to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the retrieved <see cref="GameEntity"/> if found; otherwise, null.
        /// </returns>
        Task<GameEntity> GetAsync(Guid gameId);

        /// <summary>
        /// Asynchronously deletes a game entity from the Redis data store by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the game entity to delete.</param>
        /// <returns>A task that represents the asynchronous deletion operation.</returns>
        Task DeleteAsync(Guid id);
    }
}
