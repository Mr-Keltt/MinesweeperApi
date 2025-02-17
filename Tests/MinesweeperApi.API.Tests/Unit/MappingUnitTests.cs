using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinesweeperApi.API.Models;
using MinesweeperApi.Application.Models;

namespace MinesweeperApi.API.Tests.Unit.Models
{
    /// <summary>
    /// Contains unit tests to verify that the AutoMapper configuration correctly maps between API models and business models.
    /// </summary>
    [TestClass]
    public class MappingUnitTests
    {
        // Instance of AutoMapper to be used in the tests.
        private IMapper _mapper;

        /// <summary>
        /// Initializes the AutoMapper configuration and creates a mapper instance before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Register AutoMapper profiles used for mapping requests and responses.
                cfg.AddProfile<NewGameRequestProfile>();
                cfg.AddProfile<GameInfoResponseProfile>();
                cfg.AddProfile<GameTurnRequestProfile>();
            });
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Tests that a <see cref="NewGameRequest"/> is correctly mapped to a <see cref="CreateGameModel"/>.
        /// </summary>
        [TestMethod]
        public void NewGameRequest_MapsTo_CreateGameModel()
        {
            // Arrange: Create a sample NewGameRequest with test values.
            var newGameRequest = new NewGameRequest
            {
                Width = 10,
                Height = 10,
                MinesCount = 10
            };

            // Act: Map the NewGameRequest to a CreateGameModel using AutoMapper.
            var createGameModel = _mapper.Map<CreateGameModel>(newGameRequest);

            // Assert: Verify that each property was mapped correctly.
            Assert.AreEqual(newGameRequest.Width, createGameModel.Width, "Ширина должна быть корректно отображена");
            Assert.AreEqual(newGameRequest.Height, createGameModel.Height, "Высота должна быть корректно отображена");
            Assert.AreEqual(newGameRequest.MinesCount, createGameModel.MinesCount, "Количество мин должно быть корректно отображено");
        }

        /// <summary>
        /// Tests that a <see cref="GameModel"/> is correctly mapped to a <see cref="GameInfoResponse"/>, 
        /// including the transformation of the game field.
        /// </summary>
        [TestMethod]
        public void GameModel_MapsTo_GameInfoResponse_WithFieldTransformation()
        {
            // Arrange: Create a sample GameModel with a small 2x2 field.
            // A value of -1 represents an unopened cell, and -3 represents an exploded mine.
            var gameId = Guid.NewGuid();
            var gameModel = new GameModel
            {
                Id = gameId,
                Completed = false,
                CurrentField = new int[2, 2] { { 0, -1 }, { 1, -3 } }
            };

            // Act: Map the GameModel to a GameInfoResponse.
            var response = _mapper.Map<GameInfoResponse>(gameModel);

            // Assert: Verify that open cells show their number as a string and unopened cells as a blank space.
            Assert.AreEqual("0", response.Field[0][0], "Открытая ячейка должна отображаться корректно");
            Assert.AreEqual(" ", response.Field[0][1], "Неоткрытая ячейка должна отображаться как пробел");
            Assert.AreEqual("1", response.Field[1][0], "Открытая ячейка должна отображаться корректно");
            Assert.AreEqual(" ", response.Field[1][1], "Неоткрытая ячейка должна отображаться как пробел");
        }

        /// <summary>
        /// Tests that a <see cref="GameTurnRequest"/> is correctly mapped to a <see cref="MoveModel"/>.
        /// </summary>
        [TestMethod]
        public void GameTurnRequest_MapsTo_MoveModel()
        {
            // Arrange: Create a sample GameTurnRequest with test values.
            var moveRequest = new GameTurnRequest
            {
                GameId = Guid.NewGuid(),
                Row = 2,
                Col = 3
            };

            // Act: Map the GameTurnRequest to a MoveModel.
            var moveModel = _mapper.Map<MoveModel>(moveRequest);

            // Assert: Verify that row and column values are correctly mapped.
            Assert.AreEqual(moveRequest.Row, moveModel.Row, "Номер ряда должен совпадать");
            Assert.AreEqual(moveRequest.Col, moveModel.Col, "Номер столбца должен совпадать");
        }
    }
}
