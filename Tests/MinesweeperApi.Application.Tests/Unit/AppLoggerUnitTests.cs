using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;
using Serilog.Events;
using MinesweeperApi.Application.Services.Logger;

namespace MinesweeperApi.Application.AppLoggerUnitTests
{
    /// <summary>
    /// Contains unit tests to verify that the AppLogger correctly constructs log message templates
    /// and delegates logging calls to the underlying Serilog logger.
    /// </summary>
    [TestClass]
    public class AppLoggerUnitTests
    {
        private Mock<ILogger> _mockLogger;
        private AppLogger _appLogger;

        /// <summary>
        /// Initializes the test context by creating mocks and instantiating the AppLogger.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger>();
            _appLogger = new AppLogger(_mockLogger.Object);
        }

        /// <summary>
        /// Tests that calling Write without a module prepends the system name correctly.
        /// </summary>
        [TestMethod]
        public void Write_WithNoModule_ConstructsMessageCorrectly()
        {
            // Arrange
            var messageTemplate = "Test message {Value}";
            var value = 42;
            var expectedTemplate = "[MinesweeperApi] Test message {Value} ";

            // Act
            _appLogger.Write(level: LogEventLevel.Information, messageTemplate: messageTemplate, propertyValues: new object[] { value });

            // Assert
            _mockLogger.Verify(logger =>
                logger.Write(
                    LogEventLevel.Information,
                    expectedTemplate,
                    It.Is<object[]>(props => props.Length == 1 && (int)props[0] == value)
                ),
                Times.Once);
        }

        /// <summary>
        /// Tests that calling Write with a module prepends the system and module names correctly.
        /// </summary>
        [TestMethod]
        public void Write_WithModule_ConstructsMessageCorrectly()
        {
            // Arrange
            var messageTemplate = "Test message {Value}";
            var value = 42;
            var module = new DummyModule();
            var expectedTemplate = $"[MinesweeperApi:{module}] Test message {{Value}} ";

            // Act
            _appLogger.Write(level: LogEventLevel.Information,
                             module: module,
                             messageTemplate: messageTemplate,
                             propertyValues: new object[] { value });

            // Assert
            _mockLogger.Verify(logger =>
                logger.Write(
                    LogEventLevel.Information,
                    expectedTemplate,
                    It.Is<object[]>(props => props.Length == 1 && (int)props[0] == value)
                ),
                Times.Once);
        }

        /// <summary>
        /// Tests that calling Write with an exception and no module constructs the message template correctly.
        /// </summary>
        [TestMethod]
        public void Write_WithException_NoModule_ConstructsMessageCorrectly()
        {
            // Arrange
            var messageTemplate = "Error occurred: {Error}";
            var errorValue = "Some error";
            var ex = new Exception("Test exception");
            var expectedTemplate = "[MinesweeperApi] Error occurred: {Error} ";

            // Act
            _appLogger.Write(
                level: LogEventLevel.Error,
                exception: ex,
                messageTemplate: messageTemplate,
                propertyValues: new object[] { errorValue });

            // Assert
            _mockLogger.Verify(logger =>
                logger.Write(
                    LogEventLevel.Error,
                    ex,
                    expectedTemplate,
                    It.Is<object[]>(props => props.Length == 1 && props[0].Equals(errorValue))
                ),
                Times.Once);
        }

        /// <summary>
        /// Tests that calling Write with an exception and a module constructs the message template correctly.
        /// </summary>
        [TestMethod]
        public void Write_WithException_WithModule_ConstructsMessageCorrectly()
        {
            // Arrange
            var messageTemplate = "Error occurred: {Error}";
            var errorValue = "Some error";
            var ex = new Exception("Test exception");
            var module = new DummyModule();
            var expectedTemplate = $"[MinesweeperApi:{module}] Error occurred: {{Error}} ";

            // Act
            _appLogger.Write(
                level: LogEventLevel.Error,
                exception: ex,
                module: module,
                messageTemplate: messageTemplate,
                propertyValues: new object[] { errorValue });

            // Assert
            _mockLogger.Verify(logger =>
                logger.Write(
                    LogEventLevel.Error,
                    ex,
                    expectedTemplate,
                    It.Is<object[]>(props => props.Length == 1 && props[0].Equals(errorValue))
                ),
                Times.Once);
        }

        /// <summary>
        /// Tests that calling Verbose without a module constructs the message template correctly.
        /// </summary>
        [TestMethod]
        public void Verbose_WithNoModule_ConstructsMessageCorrectly()
        {
            // Arrange
            var messageTemplate = "Verbose message {Detail}";
            var detail = "Info";
            var expectedTemplate = "[MinesweeperApi] Verbose message {Detail} ";

            // Act
            _appLogger.Verbose(messageTemplate: messageTemplate, propertyValues: new object[] { detail });

            // Assert
            _mockLogger.Verify(logger =>
                logger.Verbose(
                    expectedTemplate,
                    It.Is<object[]>(props => props.Length == 1 && props[0].Equals(detail))
                ),
                Times.Once);
        }

        /// <summary>
        /// Tests that calling Debug with a module constructs the message template correctly.
        /// </summary>
        [TestMethod]
        public void Debug_WithModule_ConstructsMessageCorrectly()
        {
            // Arrange
            var messageTemplate = "Debug message {DebugInfo}";
            var debugInfo = "MoreInfo";
            var module = new DummyModule();
            var expectedTemplate = $"[MinesweeperApi:{module}] Debug message {{DebugInfo}} ";

            // Act
            _appLogger.Debug(module: module, messageTemplate: messageTemplate, propertyValues: new object[] { debugInfo });

            // Assert
            _mockLogger.Verify(logger =>
                logger.Debug(
                    expectedTemplate,
                    It.Is<object[]>(props => props.Length == 1 && props[0].Equals(debugInfo))
                ),
                Times.Once);
        }
    }

    /// <summary>
    /// A dummy module used for testing purposes.
    /// </summary>
    public class DummyModule
    {
        public override string ToString()
        {
            return "DummyModule";
        }
    }
}
