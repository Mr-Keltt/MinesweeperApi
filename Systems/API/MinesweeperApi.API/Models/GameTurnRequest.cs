using System.ComponentModel.DataAnnotations;

namespace MinesweeperApi.API.Models;

public class GameTurnRequest
{
    [Required]
    public Guid GameId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Номер строки должен быть неотрицательным.")]
    public int Row { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Номер столбца должен быть неотрицательным.")]
    public int Col { get; set; }
}

