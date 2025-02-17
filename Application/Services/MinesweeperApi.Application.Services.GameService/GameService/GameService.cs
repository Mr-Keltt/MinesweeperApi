using System;
using System.Threading.Tasks;
using AutoMapper;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using MinesweeperApi.Application.Services.Logger;

namespace MinesweeperApi.Application.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public GameService(IRedisRepository redisRepository, IMapper mapper, IAppLogger logger)
        {
            _redisRepository = redisRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GameModel> CreateNewGameAsync(CreateGameModel newGame)
        {
            _logger.Debug(this, "Starting CreateNewGameAsync with Width={Width}, Height={Height}, MinesCount={MinesCount}", newGame.Width, newGame.Height, newGame.MinesCount);

            int width = newGame.Width;
            int height = newGame.Height;
            int minesCount = newGame.MinesCount;

            if (minesCount >= width * height || minesCount < 1)
            {
                _logger.Warning(this, "Invalid mines count: {MinesCount} for field size {Width}x{Height}", minesCount, width, height);
                throw new ArgumentException($"Количество мин должно быть между 1 и {width * height - 1}.");
            }

            newGame.CurrentField = GameServiceHelper.InitializeEmptyField(width, height);
            _logger.Debug(this, "Initialized empty game field.");

            var newGameEntity = _mapper.Map<GameEntity>(newGame);
            _logger.Debug(this, "Mapped CreateGameModel to GameEntity. Generated Id: {Id}", newGameEntity.Id);

            var gameEntity = await _redisRepository.SetAsync(newGameEntity);
            _logger.Information(this, "New game created with Id: {GameId}", gameEntity.Id);

            var gameModel = _mapper.Map<GameModel>(gameEntity);
            _logger.Debug(this, "Mapped GameEntity to GameModel.");

            return gameModel;
        }

        public async Task<GameModel> MakeMove(MoveModel move)
        {
            _logger.Debug(this, "Starting MakeMove for GameId={GameId}, Row={Row}, Col={Col}", move.GameId, move.Row, move.Col);

            Guid id = move.GameId;
            int col = move.Col;
            int row = move.Row;

            var gameEntity = await _redisRepository.GetAsync(id);
            if (gameEntity == null)
            {
                _logger.Warning(this, "Game with Id {GameId} not found.", id);
                throw new KeyNotFoundException("Игра не найдена.");
            }

            var game = _mapper.Map<GameModel>(gameEntity);
            _logger.Debug(this, "Retrieved game with Id {GameId}.", id);

            if (col < 0 || col >= game.Width || row < 0 || row >= game.Height)
            {
                _logger.Warning(this, "Move coordinates out of range: Row={Row}, Col={Col} for field size {Width}x{Height}.", row, col, game.Width, game.Height);
                throw new ArgumentException($"Координаты Col={col}, Row={row} выходят за пределы поля.");
            }

            if (game.CurrentField[row, col] >= 0 && game.CurrentField[row, col] <= 8)
            {
                _logger.Warning(this, "Cell at Row={Row}, Col={Col} is already open.", row, col);
                throw new ArgumentException("Ячейка уже открыта.");
            }

            if (GameServiceHelper.IsFieldEmpty(game.CurrentField))
            {
                _logger.Debug(this, "First move detected. Placing bombs excluding cell at Row={Row}, Col={Col}.", row, col);
                game.CurrentField = GameServiceHelper.PlaceBombs(game.CurrentField, game.MinesCount, row, col);
            }

            _logger.Debug(this, "Revealing field at Row={Row}, Col={Col}.", row, col);
            game.CurrentField = GameServiceHelper.RevealField(row, col, game.CurrentField);
            _logger.Debug(this, "Field after move: {@Field}", game.CurrentField);

            if (game.CurrentField[row, col] == -3 || GameServiceHelper.IsWin(game.CurrentField))
            {
                game.Completed = true;
                _logger.Information(this, "Game with Id {GameId} completed. Removing game from repository.", id);
                await _redisRepository.DeleteAsync(id);
            }
            else
            {
                _logger.Information(this, "Game with Id {GameId} updated in repository.", id);
                var modifiedGameEntity = _mapper.Map<GameEntity>(game);
                await _redisRepository.SetAsync(modifiedGameEntity);
            }

            return game;
        }
    }
}
