using AutoMapper;
using MinesweeperApi.Application.Models;
using Newtonsoft.Json;

namespace MinesweeperApi.API.Models;

public class GameInfoResponse
{
    /// <summary>
    /// Идентификатор игры
    /// </summary>
    [JsonProperty("game_id")]
    public Guid GameId { get; set; }

    /// <summary>
    /// Ширина игрового поля
    /// </summary>
    [JsonProperty("width")]
    public int Width { get; set; }

    /// <summary>
    /// Высота игрового поля
    /// </summary>
    [JsonProperty("height")]
    public int Height { get; set; }

    /// <summary>
    /// Количество мин на поле
    /// </summary>
    [JsonProperty("mines_count")]
    public int MinesCount { get; set; }

    /// <summary>
    /// Завершена ли игра
    /// </summary>
    [JsonProperty("completed")]
    public bool Completed { get; set; }

    /// <summary>
    /// Игровое поле – двумерный массив символов (каждая строка – массив строк)
    /// Возможные значения: " " (неоткрытая ячейка), "0"-"8", "M", "X"
    /// </summary>
    [JsonProperty("field")]
    public string[][] Field { get; set; }
}

public class GameInfoResponseProfile : Profile
{
    public GameInfoResponseProfile()
    {
        CreateMap<GameModel, GameInfoResponse>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Field, opt => opt.MapFrom<FieldToApiResolver>()); 
    }
}

public class FieldToApiResolver : IValueResolver<GameModel, GameInfoResponse, string[][]>
{
    public string[][] Resolve(GameModel source, GameInfoResponse destination, string[][] destMember, ResolutionContext context)
    {
        int rows = source.CurrentField.GetLength(0);
        int cols = source.CurrentField.GetLength(1);
        var apiField = new string[rows][];

        for (int i = 0; i < rows; i++)
        {
            apiField[i] = new string[cols];
            for (int j = 0; j < cols; j++)
            {
                int cell = source.CurrentField[i, j];
                if (cell >= 0)
                {
                    apiField[i][j] = cell.ToString();
                }
                else
                {
                    if (!source.Completed)
                    {
                        apiField[i][j] = " ";
                    }
                    else
                    {
                        if (cell == -3)
                            apiField[i][j] = "X";
                        else if (cell == -4)
                            apiField[i][j] = "M";
                        else
                            apiField[i][j] = "M";
                    }
                }
            }
        }
        return apiField;
    }
}