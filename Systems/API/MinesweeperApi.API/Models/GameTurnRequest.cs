using AutoMapper;
using MinesweeperApi.Application.Models;
using Newtonsoft.Json;

namespace MinesweeperApi.API.Models;

public class GameTurnRequest
{
    /// <summary>
    /// Идентификатор игры
    /// </summary>
    [JsonProperty("game_id")]
    public Guid GameId { get; set; }

    /// <summary>
    /// Колонка проверяемой ячейки (нумерация с нуля)
    /// </summary>
    [JsonProperty("col")]
    public int Col { get; set; }

    /// <summary>
    /// Ряд проверяемой ячейки (нумерация с нуля)
    /// </summary>
    [JsonProperty("row")]
    public int Row { get; set; }
}

public class GameTurnRequestProfile : Profile
{
    public GameTurnRequestProfile()
    {
        CreateMap<GameTurnRequest, MoveModel>();
    }
}