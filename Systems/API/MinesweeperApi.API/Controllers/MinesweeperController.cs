using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Application.Services.Logger;
using System;
using System.Threading.Tasks;

namespace MinesweeperApi.API.Controllers
{
    /// <summary>
    /// Controller for handling Minesweeper game-related API requests.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class MinesweeperController : ControllerBase
    {
        // Service for performing game operations.
        private readonly IGameService _gameService;
        // AutoMapper instance for mapping between API models and business models.
        private readonly IMapper _mapper;
        // Application logger for logging informational, warning, and error messages.
        private readonly IAppLogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinesweeperController"/> class.
        /// </summary>
        /// <param name="gameService">The game service for game operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="logger">The application logger for logging events.</param>
        public MinesweeperController(IGameService gameService, IMapper mapper, IAppLogger logger)
        {
            _gameService = gameService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new Minesweeper game.
        /// </summary>
        /// <param name="request">
        /// The request containing game parameters (width, height, and mines count).
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the created game data or an error message.
        /// </returns>
        [HttpPost("new")]
        public async Task<IActionResult> CreateGame([FromBody] NewGameRequest request)
        {
            // Log receipt of the CreateGame request with the provided parameters.
            _logger.Information(this, "Received CreateGame request with Width={Width}, Height={Height}, MinesCount={MinesCount}",
                request.Width, request.Height, request.MinesCount);

            // Validate field dimensions.
            if (request.Width <= 0 || request.Height <= 0 || request.Width > 30 || request.Height > 30)
            {
                _logger.Warning(this, "Invalid field dimensions: Width={Width}, Height={Height}", request.Width, request.Height);
                return BadRequest(new ErrorResponse { Error = "Ширина и высота должны быть больше 0 и не больше 30." });
            }

            // Validate the mines count.
            if (request.MinesCount < 1 || request.MinesCount >= request.Width * request.Height)
            {
                _logger.Warning(this, "Invalid mines count: MinesCount={MinesCount} for field {Width}x{Height}",
                    request.MinesCount, request.Width, request.Height);
                return BadRequest(new ErrorResponse { Error = $"Количество мин должно быть от 1 до {request.Width * request.Height - 1}." });
            }

            try
            {
                // Map the incoming NewGameRequest to the business model CreateGameModel.
                var createGameModel = _mapper.Map<CreateGameModel>(request);
                // Create a new game using the game service.
                var game = await _gameService.CreateNewGameAsync(createGameModel);
                // Map the created game business model to the API response model.
                var response = _mapper.Map<GameInfoResponse>(game);

                _logger.Information(this, "Successfully created game with GameId={GameId}", response.GameId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during game creation.
                _logger.Error(ex, this, "Error occurred during CreateGame");
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
        }

        /// <summary>
        /// Processes a move (turn) in an existing game.
        /// </summary>
        /// <param name="request">
        /// The request containing move parameters: game_id, row, and col.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the updated game data or an error message.
        /// </returns>
        [HttpPost("turn")]
        public async Task<IActionResult> MakeMove([FromBody] GameTurnRequest request)
        {
            // Log receipt of the MakeMove request with the provided parameters.
            _logger.Information(this, "Received MakeMove request with GameId={GameId}, Row={Row}, Col={Col}",
                request.GameId, request.Row, request.Col);

            try
            {
                // Map the incoming GameTurnRequest to the business model MoveModel.
                var moveModel = _mapper.Map<MoveModel>(request);
                // Process the move using the game service.
                var game = await _gameService.MakeMove(moveModel);
                // Map the updated game business model to the API response model.
                var response = _mapper.Map<GameInfoResponse>(game);

                _logger.Information(this, "Move processed successfully for GameId={GameId}", response.GameId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during move processing.
                _logger.Error(ex, this, "Error occurred during MakeMove for GameId={GameId}", request.GameId);
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
        }
    }
}
