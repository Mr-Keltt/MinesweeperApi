using AutoMapper;
using MinesweeperApi.Application.Models;
using Newtonsoft.Json;

namespace MinesweeperApi.API.Models;

public class NewGameRequest
{
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
}

public class NewGameRequestProfile : Profile
{
    public NewGameRequestProfile()
    {
        CreateMap<NewGameRequest, CreateGameModel>();
    }
}