using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Application.Services.Logger;

namespace MinesweeperApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MinesweeperController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IMapper _mapper;
    private readonly IAppLogger _logger;

    public MinesweeperController(IGameService gameService, IMapper mapper, IAppLogger logger)
    {
        _gameService = gameService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Создает новую игру Сапёр.
    /// </summary>
    /// <param name="request">Параметры игры: ширина, высота и количество мин.</param>
    /// <returns>Данные созданной игры.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request)
    {
        _logger.Information(this, "Received CreateGame request with Width={Width}, Height={Height}, MinesCount={MinesCount}",
            request.Width, request.Height, request.MinesCount);

        // Валидация входных параметров
        if (request.Width <= 0 || request.Height <= 0 || request.Width > 30 || request.Height > 30)
        {
            _logger.Warning(this, "Invalid field dimensions: Width={Width}, Height={Height}", request.Width, request.Height);
            return BadRequest("Ширина и высота должны быть больше 0 и не больше 30.");
        }
        if (request.MinesCount < 1 || request.MinesCount >= request.Width * request.Height)
        {
            _logger.Warning(this, "Invalid mines count: MinesCount={MinesCount} for field {Width}x{Height}",
                request.MinesCount, request.Width, request.Height);
            return BadRequest($"Количество мин должно быть от 1 до {request.Width * request.Height - 1}.");
        }

        try
        {
            var createGameModel = _mapper.Map<CreateGameModel>(request);
            var game = await _gameService.CreateNewGameAsync(createGameModel);
            var response = _mapper.Map<GameResponse>(game);

            _logger.Information(this, "Successfully created game with GameId={GameId}", response.GameId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, this, "Error occurred during CreateGame");
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }

    /// <summary>
    /// Делает ход в игре.
    /// </summary>
    /// <param name="request">Параметры хода: game_id, row и col.</param>
    /// <returns>Обновленные данные игры.</returns>
    [HttpPost("move")]
    public async Task<IActionResult> MakeMove([FromBody] MoveRequest request)
    {
        _logger.Information(this, "Received MakeMove request with GameId={GameId}, Row={Row}, Col={Col}",
            request.GameId, request.Row, request.Col);

        try
        {
            var moveModel = _mapper.Map<MoveModel>(request);
            var game = await _gameService.MakeMove(moveModel);
            var response = _mapper.Map<GameResponse>(game);

            _logger.Information(this, "Move processed successfully for GameId={GameId}", response.GameId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, this, "Error occurred during MakeMove for GameId={GameId}", request.GameId);
            return BadRequest(new ErrorResponse { Error = ex.Message });
        }
    }
}