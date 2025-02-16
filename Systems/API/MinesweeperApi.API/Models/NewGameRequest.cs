using System.ComponentModel.DataAnnotations;

namespace MinesweeperApi.API.Models;

public class NewGameRequest
{
    [Required]
    [Range(1, 30, ErrorMessage = "Ширина должна быть от 1 до 30.")]
    public int Width { get; set; }

    [Required]
    [Range(1, 30, ErrorMessage = "Высота должна быть от 1 до 30.")]
    public int Height { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Количество мин должно быть положительным.")]
    public int MinesCount { get; set; }
}

