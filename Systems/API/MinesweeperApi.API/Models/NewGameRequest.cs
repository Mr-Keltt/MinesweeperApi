using AutoMapper;
using MinesweeperApi.Application.Models;
using Newtonsoft.Json;

namespace MinesweeperApi.API.Models
{
    /// <summary>
    /// Represents a request to create a new Minesweeper game.
    /// </summary>
    public class NewGameRequest
    {
        /// <summary>
        /// Gets or sets the width of the game field.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game field.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the number of mines to be placed on the game field.
        /// </summary>
        [JsonProperty("mines_count")]
        public int MinesCount { get; set; }
    }

    /// <summary>
    /// AutoMapper profile for mapping <see cref="NewGameRequest"/> to <see cref="CreateGameModel"/>.
    /// </summary>
    public class NewGameRequestProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewGameRequestProfile"/> class.
        /// Configures the mapping between <see cref="NewGameRequest"/> and <see cref="CreateGameModel"/>.
        /// </summary>
        public NewGameRequestProfile()
        {
            CreateMap<NewGameRequest, CreateGameModel>();
        }
    }
}
