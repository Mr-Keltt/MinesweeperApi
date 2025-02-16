using MinesweeperApi.Application.Models;

namespace MinesweeperApi.Application.Services.GameService;

public interface IGameService
{
    Task<GameModel> CreateNewGameAsync(CreateGameModel newGame);
    Task<GameModel> MakeMove(MoveModel move);
}