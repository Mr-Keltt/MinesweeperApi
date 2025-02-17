using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Models;

namespace MinesweeperApi.API.Tests.Unit.Models;

[TestClass]
public class MappingUnitTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NewGameRequestProfile>();
            cfg.AddProfile<GameInfoResponseProfile>();
            cfg.AddProfile<GameTurnRequestProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void NewGameRequest_MapsTo_CreateGameModel()
    {
        // Arrange
        var newGameRequest = new NewGameRequest
        {
            Width = 10,
            Height = 10,
            MinesCount = 10
        };

        // Act
        var createGameModel = _mapper.Map<CreateGameModel>(newGameRequest);

        // Assert
        Assert.AreEqual(newGameRequest.Width, createGameModel.Width, "Ширина должна быть корректно отображена");
        Assert.AreEqual(newGameRequest.Height, createGameModel.Height, "Высота должна быть корректно отображена");
        Assert.AreEqual(newGameRequest.MinesCount, createGameModel.MinesCount, "Количество мин должно быть корректно отображено");
    }

    [TestMethod]
    public void GameModel_MapsTo_GameInfoResponse_WithFieldTransformation()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameModel = new GameModel
        {
            Id = gameId,
            Completed = false,
            CurrentField = new int[2, 2] { { 0, -1 }, { 1, -3 } }
        };

        // Act
        var response = _mapper.Map<GameInfoResponse>(gameModel);

        // Asser
        Assert.AreEqual("0", response.Field[0][0], "Открытая ячейка должна отображаться корректно");
        Assert.AreEqual(" ", response.Field[0][1], "Неоткрытая ячейка должна отображаться как пробел");
        Assert.AreEqual("1", response.Field[1][0], "Открытая ячейка должна отображаться корректно");
        Assert.AreEqual(" ", response.Field[1][1], "Неоткрытая ячейка должна отображаться как пробел");
    }

    [TestMethod]
    public void GameTurnRequest_MapsTo_MoveModel()
    {
        // Arrange
        var moveRequest = new GameTurnRequest
        {
            GameId = Guid.NewGuid(),
            Row = 2,
            Col = 3
        };

        // Act
        var moveModel = _mapper.Map<MoveModel>(moveRequest);

        // Assert
        Assert.AreEqual(moveRequest.Row, moveModel.Row, "Номер ряда должен совпадать");
        Assert.AreEqual(moveRequest.Col, moveModel.Col, "Номер столбца должен совпадать");
    }
}
