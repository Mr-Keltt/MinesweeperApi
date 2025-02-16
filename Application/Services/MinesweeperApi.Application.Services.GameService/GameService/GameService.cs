using AutoMapper;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Infrastructure.Data.Entities;
using MinesweeperApi.Infrastructure.Repositories;
using Newtonsoft.Json;

namespace MinesweeperApi.Application.Services.GameService;

public class GameService : IGameService
{
    private readonly IRedisRepository _redisRepository;
    private readonly IMapper _mapper;

    public GameService(IRedisRepository redisRepository, IMapper mapper)
    {
        _redisRepository = redisRepository;
        this._mapper = mapper;
    }

    public async Task<GameModel> CreateNewGameAsync(CreateGameModel newGame)
    {
        int width = newGame.Width;
        int height = newGame.Height;
        int minesCount = newGame.MinesCount;

        if (minesCount >= width * height || minesCount < 1)
        {
            throw new ArgumentException($"Количество мин быть между 1 и {width * height}.");
        }

        newGame.CurrentField = GameServiceHelper.InitializeField(width, height, minesCount);

        var newGameEntity = _mapper.Map<GameEntity>(newGame);
        var gameEntity = await _redisRepository.SetAsync(newGameEntity);

        return _mapper.Map<GameModel>(gameEntity);
    }

    public async Task<GameModel> MakeMove(MoveModel move)
    {
        Guid id = move.GameId;
        int x = move.Col;
        int y = move.Row;

        var gameEntity = _redisRepository.GetAsync(id);
        var game = _mapper.Map<GameModel>(gameEntity);

        if (game == null)
        {
            throw new KeyNotFoundException("Игра не найдена.");
        }
            
        if (x < 0 || x >= game.Width || y < 0 || y >= game.Height)
        {
            throw new ArgumentException($"Координаты Col={x}, Row={y} выходят за приделы поля.");
        }

        if (game.CurrentField[y, x] == -1)
        {
            game.Completed = true;
            await _redisRepository.DeleteAsync(id);
        }

        // TODO: Логика открытия поля по заданным координатам

        var modifiedGameEntity = _mapper.Map<GameEntity>(game);
        await _redisRepository.SetAsync(modifiedGameEntity);

        return game;
    }
}
