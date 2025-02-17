using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.API.Controllers;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Models;
using MinesweeperApi.Application.Services.GameService;
using MinesweeperApi.Application.Services.Logger;
using Moq;

namespace MinesweeperApi.API.Tests.Unit;

[TestClass]
public class MinesweeperControllerUnitTests
{
    private Mock<IGameService> _gameServiceMock;
    private IMapper _mapper;
    private Mock<IAppLogger> _loggerMock;
    private MinesweeperController _controller;

    [TestInitialize]
    public void Setup()
    {
        _gameServiceMock = new Mock<IGameService>();
        _loggerMock = new Mock<IAppLogger>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NewGameRequest, CreateGameModel>();
            cfg.CreateMap<GameModel, GameInfoResponse>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<GameTurnRequest, MoveModel>();
        });
        _mapper = config.CreateMapper();

        _controller = new MinesweeperController(_gameServiceMock.Object, _mapper, _loggerMock.Object);
    }

    [TestMethod]
    public async Task CreateGame_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new NewGameRequest { Width = 10, Height = 10, MinesCount = 10 };
        var gameModel = new GameModel { Id = Guid.NewGuid()};
        _gameServiceMock.Setup(s => s.CreateNewGameAsync(It.IsAny<CreateGameModel>()))
                        .ReturnsAsync(gameModel);

        // Act
        var result = await _controller.CreateGame(request);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult, "Ожидается OkObjectResult");
        var response = okResult.Value as GameInfoResponse;
        Assert.IsNotNull(response, "Ожидается корректный объект ответа");
        Assert.AreEqual(gameModel.Id, response.GameId, "Идентификаторы игры должны совпадать");
    }

    [TestMethod]
    public async Task CreateGame_InvalidDimensions_ReturnsBadRequestResult()
    {
        // Arrange
        var request = new NewGameRequest { Width = 0, Height = 0, MinesCount = 1 };

        // Act
        var result = await _controller.CreateGame(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult), "Ожидается BadRequestObjectResult");
    }

    [TestMethod]
    public async Task MakeMove_ValidParameters_ReturnsOkResult()
    {
        // Arrange
        var newGameRequest = new NewGameRequest { Width = 10, Height = 10, MinesCount = 10 };
        var gameModel = new GameModel { Id = Guid.NewGuid() };
        _gameServiceMock.Setup(s => s.CreateNewGameAsync(It.IsAny<CreateGameModel>()))
                        .ReturnsAsync(gameModel);

        var createResult = await _controller.CreateGame(newGameRequest);
        var okResult = createResult as OkObjectResult;
        var createdGame = okResult.Value as GameInfoResponse;

        _gameServiceMock.Setup(s => s.MakeMove(It.IsAny<MoveModel>()))
                        .ReturnsAsync(gameModel);

        var moveRequest = new GameTurnRequest
        {
            GameId = createdGame.GameId,
            Row = 0,
            Col = 0
        };

        // Act
        var moveResult = await _controller.MakeMove(moveRequest);

        // Assert
        var moveOkResult = moveResult as OkObjectResult;
        Assert.IsNotNull(moveOkResult, "Ожидается OkObjectResult для MakeMove");
        var moveResponse = moveOkResult.Value as GameInfoResponse;
        Assert.IsNotNull(moveResponse, "Ожидается корректный объект ответа после хода");
        Assert.AreEqual(createdGame.GameId, moveResponse.GameId, "Идентификатор игры должен сохраняться");
    }
}