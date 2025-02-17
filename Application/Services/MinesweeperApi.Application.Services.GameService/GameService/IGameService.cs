using System.Threading.Tasks;
using MinesweeperApi.Application.Models;

namespace MinesweeperApi.Application.Services.GameService
{
    /// <summary>
    /// Defines the contract for game-related operations including game creation and processing moves.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Creates a new game with the specified configuration.
        /// </summary>
        /// <param name="newGame">The model containing the configuration details for the new game.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="GameModel"/>
        /// representing the newly created game.
        /// </returns>
        Task<GameModel> CreateNewGameAsync(CreateGameModel newGame);

        /// <summary>
        /// Processes a move for an existing game by updating its state accordingly.
        /// </summary>
        /// <param name="move">The model containing the move details, including game identifier and cell coordinates.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a <see cref="GameModel"/>
        /// representing the game state after the move has been processed.
        /// </returns>
        Task<GameModel> MakeMove(MoveModel move);
    }
}
