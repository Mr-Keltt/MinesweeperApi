using Newtonsoft.Json;

namespace MinesweeperApi.API.Models;

public class ErrorResponse
{
    /// <summary>
    /// Описание ошибки
    /// </summary>
    [JsonProperty("error")]
    public string Error { get; set; }
}