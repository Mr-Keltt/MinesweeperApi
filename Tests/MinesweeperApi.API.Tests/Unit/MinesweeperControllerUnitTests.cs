using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.API.Controllers;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Application.Services.Logger;
using Moq;
using System;
using System.Threading.Tasks;

namespace MinesweeperApi.API.Tests.Unit
{
    /// <summary>
    /// Contains unit tests for the <see cref="MinesweeperController"/>, verifying its behavior for creating games and making moves.
    /// </summary>
    [TestClass]
    public class MinesweeperControllerUnitTests
    {
        // Mocked instance of the game service.
        private Mock<IGameService> _gameServiceMock;
        // AutoMapper instance for mapping between API and business models.
        private IMapper _mapper;
        // Mocked instance of the application logger.
        private Mock<IAppLogger> _loggerMock;
        // The controller under test.
        private MinesweeperController _controller;

        /// <summary>
        /// Initializes the test context, including mocks and AutoMapper configuration, before each test method.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Create mocks for IGameService and IAppLogger.
            _gameServiceMock = new Mock<IGameService>();
            _loggerMock = new Mock<IAppLogger>();

            // Configure AutoMapper with minimal profiles for mapping requests and responses.
            var config = new MapperConfiguration(cfg =>
            {
                // Map NewGameRequest to CreateGameModel.
                cfg.CreateMap<NewGameRequest, CreateGameModel>();
                // Map GameModel to GameInfoResponse, explicitly mapping the GameId property.
                cfg.CreateMap<GameModel, GameInfoResponse>()
                    .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id));
                // Map GameTurnRequest to MoveModel.
                cfg.CreateMap<GameTurnRequest, MoveModel>();
            });
            _mapper = config.CreateMapper();

            // Initialize the MinesweeperController with the mocked dependencies.
            _controller = new MinesweeperController(_gameServiceMock.Object, _mapper, _loggerMock.Object);
        }

        /// <summary>
        /// Tests that a valid NewGameRequest is correctly processed by the controller, returning an OkObjectResult with a valid GameInfoResponse.
        /// </summary>
        [TestMethod]
        public async Task CreateGame_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            // Create a valid new game request.
            var request = new NewGameRequest { Width = 10, Height = 10, MinesCount = 10 };
            // Create a sample GameModel representing the created game.
            var gameModel = new GameModel { Id = Guid.NewGuid() };
            // Setup the game service mock to return the sample gameModel when CreateNewGameAsync is called.
            _gameServiceMock.Setup(s => s.CreateNewGameAsync(It.IsAny<CreateGameModel>()))
                            .ReturnsAsync(gameModel);

            // Act
            // Call the CreateGame action on the controller.
            var result = await _controller.CreateGame(request);

            // Assert
            // Verify that the result is an OkObjectResult.
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Ожидается OkObjectResult");
            // Verify that the value in the result is a valid GameInfoResponse.
            var response = okResult.Value as GameInfoResponse;
            Assert.IsNotNull(response, "Ожидается корректный объект ответа");
            // Verify that the GameId from the created game matches the response.
            Assert.AreEqual(gameModel.Id, response.GameId, "Идентификаторы игры должны совпадать");
        }

        /// <summary>
        /// Tests that a NewGameRequest with invalid field dimensions results in a BadRequestObjectResult.
        /// </summary>
        [TestMethod]
        public async Task CreateGame_InvalidDimensions_ReturnsBadRequestResult()
        {
            // Arrange
            // Create an invalid new game request with zero dimensions.
            var request = new NewGameRequest { Width = 0, Height = 0, MinesCount = 1 };

            // Act
            // Call the CreateGame action on the controller.
            var result = await _controller.CreateGame(request);

            // Assert
            // Verify that the controller returns a BadRequestObjectResult.
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult), "Ожидается BadRequestObjectResult");
        }

        /// <summary>
        /// Tests that a valid GameTurnRequest is correctly processed by the controller, returning an OkObjectResult with a valid GameInfoResponse.
        /// </summary>
        [TestMethod]
        public async Task MakeMove_ValidParameters_ReturnsOkResult()
        {
            // Arrange
            // First, create a new game using a valid request.
            var newGameRequest = new NewGameRequest { Width = 10, Height = 10, MinesCount = 10 };
            var gameModel = new GameModel { Id = Guid.NewGuid() };
            _gameServiceMock.Setup(s => s.CreateNewGameAsync(It.IsAny<CreateGameModel>()))
                            .ReturnsAsync(gameModel);

            // Create the game and retrieve the created GameInfoResponse.
            var createResult = await _controller.CreateGame(newGameRequest);
            var okResult = createResult as OkObjectResult;
            var createdGame = okResult.Value as GameInfoResponse;

            // Setup the game service mock to return the gameModel when a move is made.
            _gameServiceMock.Setup(s => s.MakeMove(It.IsAny<MoveModel>()))
                            .ReturnsAsync(gameModel);

            // Create a valid move request using the created game's GameId.
            var moveRequest = new GameTurnRequest
            {
                GameId = createdGame.GameId,
                Row = 0,
                Col = 0
            };

            // Act
            // Call the MakeMove action on the controller.
            var moveResult = await _controller.MakeMove(moveRequest);

            // Assert
            // Verify that the result is an OkObjectResult.
            var moveOkResult = moveResult as OkObjectResult;
            Assert.IsNotNull(moveOkResult, "Ожидается OkObjectResult для MakeMove");
            // Verify that the response contains a valid GameInfoResponse.
            var moveResponse = moveOkResult.Value as GameInfoResponse;
            Assert.IsNotNull(moveResponse, "Ожидается корректный объект ответа после хода");
            // Verify that the GameId remains unchanged after the move.
            Assert.AreEqual(createdGame.GameId, moveResponse.GameId, "Идентификатор игры должен сохраняться");
        }
    }
}
