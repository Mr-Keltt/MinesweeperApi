using System;
using System.Threading.Tasks;
using AutoMapper;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.Logger;

namespace MinesweeperApi.Application.Services.GameService
{
    /// <summary>
    /// Provides game-related operations such as creating a new game and processing moves.
    /// </summary>
    public class GameService : IGameService
    {
        // Repository for interacting with the Redis data store.
        private readonly IRedisRepository _redisRepository;

        // AutoMapper instance for mapping between models and entities.
        private readonly IMapper _mapper;

        // Application logger for logging debug, information, and warning messages.
        private readonly IAppLogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="redisRepository">The repository for Redis data operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="logger">The application logger for logging messages.</param>
        public GameService(IRedisRepository redisRepository, IMapper mapper, IAppLogger logger)
        {
            _redisRepository = redisRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new game with the provided configuration.
        /// </summary>
        /// <param name="newGame">The model containing game configuration details.</param>
        /// <returns>A <see cref="GameModel"/> representing the newly created game.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of mines is not within the valid range for the field size.
        /// </exception>
        public async Task<GameModel> CreateNewGameAsync(CreateGameModel newGame)
        {
            // Log the initiation of the new game creation process with provided parameters.
            _logger.Debug(this, "Starting CreateNewGameAsync with Width={Width}, Height={Height}, MinesCount={MinesCount}", newGame.Width, newGame.Height, newGame.MinesCount);

            int width = newGame.Width;
            int height = newGame.Height;
            int minesCount = newGame.MinesCount;

            // Validate that the number of mines is within the acceptable range.
            if (minesCount >= width * height || minesCount < 1)
            {
                _logger.Warning(this, "Invalid mines count: {MinesCount} for field size {Width}x{Height}", minesCount, width, height);
                throw new ArgumentException($"Количество мин должно быть между 1 и {width * height - 1}.");
            }

            // Initialize an empty game field.
            newGame.CurrentField = GameServiceHelper.InitializeEmptyField(width, height);
            _logger.Debug(this, "Initialized empty game field.");

            // Map the creation model to a persistent game entity.
            var newGameEntity = _mapper.Map<GameEntity>(newGame);
            _logger.Debug(this, "Mapped CreateGameModel to GameEntity. Generated Id: {Id}", newGameEntity.Id);

            // Store the new game entity in the Redis repository.
            var gameEntity = await _redisRepository.SetAsync(newGameEntity);
            _logger.Information(this, "New game created with Id: {GameId}", gameEntity.Id);

            // Map the stored entity back to a game model to return to the caller.
            var gameModel = _mapper.Map<GameModel>(gameEntity);
            _logger.Debug(this, "Mapped GameEntity to GameModel.");

            return gameModel;
        }

        /// <summary>
        /// Processes a move on the specified game and updates its state.
        /// </summary>
        /// <param name="move">The model containing move details such as game identifier and coordinates.</param>
        /// <returns>A <see cref="GameModel"/> representing the updated game state after the move.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the game is not found in the repository.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the move is out of range or if the cell is already open.
        /// </exception>
        public async Task<GameModel> MakeMove(MoveModel move)
        {
            // Log the initiation of the move process with move parameters.
            _logger.Debug(this, "Starting MakeMove for GameId={GameId}, Row={Row}, Col={Col}", move.GameId, move.Row, move.Col);

            Guid id = move.GameId;
            int col = move.Col;
            int row = move.Row;

            // Retrieve the game entity from the repository using the game ID.
            var gameEntity = await _redisRepository.GetAsync(id);
            if (gameEntity == null)
            {
                _logger.Warning(this, "Game with Id {GameId} not found.", id);
                throw new KeyNotFoundException("Игра не найдена.");
            }

            // Map the retrieved entity to a game model.
            var game = _mapper.Map<GameModel>(gameEntity);
            _logger.Debug(this, "Retrieved game with Id {GameId}.", id);

            // Validate that the move coordinates are within the bounds of the game field.
            if (col < 0 || col >= game.Width || row < 0 || row >= game.Height)
            {
                _logger.Warning(this, "Move coordinates out of range: Row={Row}, Col={Col} for field size {Width}x{Height}.", row, col, game.Width, game.Height);
                throw new ArgumentException($"Координаты Col={col}, Row={row} выходят за пределы поля.");
            }

            // Check if the cell is already open; a cell value between 0 and 8 indicates an open cell.
            if (game.CurrentField[row, col] >= 0 && game.CurrentField[row, col] <= 8)
            {
                _logger.Warning(this, "Cell at Row={Row}, Col={Col} is already open.", row, col);
                throw new ArgumentException("Ячейка уже открыта.");
            }

            // For the first move, if the field is empty, place bombs excluding the initial move cell.
            if (GameServiceHelper.IsFieldEmpty(game.CurrentField))
            {
                _logger.Debug(this, "First move detected. Placing bombs excluding cell at Row={Row}, Col={Col}.", row, col);
                game.CurrentField = GameServiceHelper.PlaceBombs(game.CurrentField, game.MinesCount, row, col);
            }

            // Reveal the game field starting from the specified cell.
            _logger.Debug(this, "Revealing field at Row={Row}, Col={Col}.", row, col);
            game.CurrentField = GameServiceHelper.RevealField(row, col, game.CurrentField);
            _logger.Debug(this, "Field after move: {@Field}", game.CurrentField);

            // Check if the move resulted in a bomb hit or if the game has been won.
            if (game.CurrentField[row, col] == -3 || GameServiceHelper.IsWin(game.CurrentField))
            {
                // Mark the game as completed.
                game.Completed = true;
                _logger.Information(this, "Game with Id {GameId} completed. Removing game from repository.", id);

                // Remove the completed game from the repository.
                await _redisRepository.DeleteAsync(id);
            }
            else
            {
                // Update the game state in the repository.
                _logger.Information(this, "Game with Id {GameId} updated in repository.", id);
                var modifiedGameEntity = _mapper.Map<GameEntity>(game);
                await _redisRepository.SetAsync(modifiedGameEntity);
            }

            return game;
        }
    }
}
