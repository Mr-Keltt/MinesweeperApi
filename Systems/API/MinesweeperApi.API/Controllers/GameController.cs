using Microsoft.AspNetCore.Mvc;
using MinesweeperApi.API.Models;

namespace MinesweeperApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MinesweeperController : ControllerBase
{
    public MinesweeperController()
    {
        // TODO: Инициализация зависимостей, если необходимо
    }

    /// <summary>
    /// The beginning of a new game.
    /// </summary>
    /// <param name="request">The parameters of the new game.</param>
    /// <returns>Information about the created game.</returns>
    [HttpPost("new")]
    [ProducesResponseType(typeof(GameInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult NewGame([FromBody] NewGameRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Некорректные параметры запроса."
            });
        }

        if (request.MinesCount >= request.Width * request.Height)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Количество мин не может быть больше или равно количеству ячеек на поле."
            });
        }

        // TODO: Заглушка для бизнес-логики создания новой игры
        // Здесь должна быть логика создания новой игры и сохранения ее состояния
        var gameId = Guid.NewGuid(); // Генерация нового идентификатора игры
        var field = new string[request.Height, request.Width];
        // Инициализация поля пустыми ячейками (например, пробелами)
        for (int i = 0; i < request.Height; i++)
        {
            for (int j = 0; j < request.Width; j++)
            {
                field[i, j] = " ";
            }
        }

        var response = new GameInfoResponse
        {
            GameId = gameId,
            Width = request.Width,
            Height = request.Height,
            MinesCount = request.MinesCount,
            Completed = false,
            Field = field
        };

        return Ok(response);
    }

    /// <summary>
    /// The user's progress.
    /// </summary>
    /// <param name="request">Stroke Parameters.</param>
    /// <returns>Updated information about the game.</returns>
    [HttpPost("turn")]
    [ProducesResponseType(typeof(GameInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult MakeTurn([FromBody] GameTurnRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Некорректные параметры запроса."
            });
        }

        // TODO: Заглушка для бизнес-логики обработки хода
        // Здесь должна быть логика получения состояния игры по gameId,
        // проверки возможности хода, обновления состояния игры и сохранения изменений

        // Пример проверки выхода за пределы поля
        // Предполагается, что размеры поля известны (например, получены из хранилища вместе с состоянием игры)
        int width = 10; // Заменить на реальное значение ширины из состояния игры
        int height = 10; // Заменить на реальное значение высоты из состояния игры

        if (request.Row >= height || request.Col >= width)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Координаты хода выходят за пределы игрового поля."
            });
        }

        // Пример обновленного поля после хода
        var field = new string[height, width];
        // Логика обновления поля на основе хода пользователя
        // ...

        var response = new GameInfoResponse
        {
            GameId = request.GameId,
            Width = width,
            Height = height,
            MinesCount = 10, // Заменить на реальное значение из состояния игры
            Completed = false, // Заменить на реальное значение из состояния игры
            Field = field
        };

        return Ok(response);
    }
}

