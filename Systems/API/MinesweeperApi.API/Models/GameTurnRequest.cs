using AutoMapper;
using MinesweeperApi.Application.Models;
using Newtonsoft.Json;
using System;

namespace MinesweeperApi.API.Models
{
    /// <summary>
    /// Represents a request to perform a move (turn) in a Minesweeper game.
    /// </summary>
    public class GameTurnRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the game.
        /// </summary>
        [JsonProperty("game_id")]
        public Guid GameId { get; set; }

        /// <summary>
        /// Gets or sets the column index of the cell to be checked (zero-based indexing).
        /// </summary>
        [JsonProperty("col")]
        public int Col { get; set; }

        /// <summary>
        /// Gets or sets the row index of the cell to be checked (zero-based indexing).
        /// </summary>
        [JsonProperty("row")]
        public int Row { get; set; }
    }

    /// <summary>
    /// AutoMapper profile for mapping <see cref="GameTurnRequest"/> to <see cref="MoveModel"/>.
    /// </summary>
    public class GameTurnRequestProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameTurnRequestProfile"/> class.
        /// Configures the mapping from <see cref="GameTurnRequest"/> to <see cref="MoveModel"/>.
        /// </summary>
        public GameTurnRequestProfile()
        {
            CreateMap<GameTurnRequest, MoveModel>();
        }
    }
}
