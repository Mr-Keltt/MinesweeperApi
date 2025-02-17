using AutoMapper;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace MinesweeperApi.Application.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;

        public GameService(IRedisRepository redisRepository, IMapper mapper)
        {
            _redisRepository = redisRepository;
            _mapper = mapper;
        }

        public async Task<GameModel> CreateNewGameAsync(CreateGameModel newGame)
        {
            int width = newGame.Width;
            int height = newGame.Height;
            int minesCount = newGame.MinesCount;

            if (minesCount >= width * height || minesCount < 1)
                throw new ArgumentException($"Количество мин быть между 1 и {width * height}.");

            newGame.CurrentField = GameServiceHelper.InitializeField(width, height, minesCount);

            var newGameEntity = _mapper.Map<GameEntity>(newGame);
            var gameEntity = await _redisRepository.SetAsync(newGameEntity);

            return _mapper.Map<GameModel>(gameEntity);
        }

        public async Task<GameModel> MakeMove(MoveModel move)
        {
            Guid id = move.GameId;
            int col = move.Col;
            int row = move.Row;

            var gameEntity = await _redisRepository.GetAsync(id);
            var game = _mapper.Map<GameModel>(gameEntity);

            if (game == null)
                throw new KeyNotFoundException("Игра не найдена.");

            if (col < 0 || col >= game.Width || row < 0 || row >= game.Height)
                throw new ArgumentException($"Координаты Col={col}, Row={row} выходят за пределы поля.");

            if (game.CurrentField[row, col] >= 0 && game.CurrentField[row, col] <= 8)
                throw new ArgumentException("Ячейка уже открыта.");

            game.CurrentField = GameServiceHelper.RevealField(row, col, game.CurrentField);

            if (game.CurrentField[row, col] == -3 || GameServiceHelper.IsWin(game.CurrentField))
            {
                game.Completed = true;
                await _redisRepository.DeleteAsync(id);
            }
            else
            {
                var modifiedGameEntity = _mapper.Map<GameEntity>(game);
                await _redisRepository.SetAsync(modifiedGameEntity);
            }

            return game;
        }
    }
}
