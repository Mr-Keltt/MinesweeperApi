using AutoMapper;
using MinesweeperApi.Application.Models;
using Newtonsoft.Json;

namespace MinesweeperApi.API.Models
{
    /// <summary>
    /// Represents the API response model containing game information.
    /// </summary>
    public class GameInfoResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the game.
        /// </summary>
        [JsonProperty("game_id")]
        public Guid GameId { get; set; }

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
        /// Gets or sets the number of mines on the field.
        /// </summary>
        [JsonProperty("mines_count")]
        public int MinesCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is completed.
        /// </summary>
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the game field represented as a two-dimensional array of strings.
        /// Each cell's possible values are:
        /// " " (unopened cell), "0" to "8", "M" for mine, or "X" for an exploded mine.
        /// </summary>
        [JsonProperty("field")]
        public string[][] Field { get; set; }
    }

    /// <summary>
    /// AutoMapper profile for mapping <see cref="GameModel"/> to <see cref="GameInfoResponse"/>.
    /// </summary>
    public class GameInfoResponseProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInfoResponseProfile"/> class.
        /// Configures the mapping from <see cref="GameModel"/> to <see cref="GameInfoResponse"/>.
        /// </summary>
        public GameInfoResponseProfile()
        {
            CreateMap<GameModel, GameInfoResponse>()
                // Map the GameId property directly.
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
                // Map the game field using a custom resolver.
                .ForMember(dest => dest.Field, opt => opt.MapFrom<FieldToApiResolver>());
        }
    }

    /// <summary>
    /// Custom AutoMapper resolver to convert a game field from a two-dimensional integer array
    /// to a two-dimensional string array suitable for API responses.
    /// </summary>
    public class FieldToApiResolver : IValueResolver<GameModel, GameInfoResponse, string[][]>
    {
        /// <summary>
        /// Resolves the game field mapping from <see cref="GameModel.CurrentField"/> to the API response field.
        /// </summary>
        /// <param name="source">The source game model containing the current field as a two-dimensional integer array.</param>
        /// <param name="destination">The destination game info response model.</param>
        /// <param name="destMember">The destination member (unused in this resolver).</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>A two-dimensional array of strings representing the game field.</returns>
        public string[][] Resolve(GameModel source, GameInfoResponse destination, string[][] destMember, ResolutionContext context)
        {
            int rows = source.CurrentField.GetLength(0);
            int cols = source.CurrentField.GetLength(1);
            var apiField = new string[rows][];

            // Iterate through each cell in the source field to convert it to a string representation.
            for (int i = 0; i < rows; i++)
            {
                apiField[i] = new string[cols];
                for (int j = 0; j < cols; j++)
                {
                    int cell = source.CurrentField[i, j];

                    // If the cell has a non-negative value, it represents the number of adjacent mines.
                    if (cell >= 0)
                    {
                        apiField[i][j] = cell.ToString();
                    }
                    else
                    {
                        // For negative cell values, decide the display value based on game completion.
                        if (!source.Completed)
                        {
                            // If the game is not completed, unopened cells are represented by a blank space.
                            apiField[i][j] = " ";
                        }
                        else
                        {
                            // When the game is completed, mark the cells accordingly.
                            // A cell value of -3 indicates an exploded mine.
                            // A cell value of -4 or any other negative value is treated as a mine.
                            if (cell == -3)
                                apiField[i][j] = "X";
                            else // covers cell == -4 and any other negative mine indicator.
                                apiField[i][j] = "M";
                        }
                    }
                }
            }
            return apiField;
        }
    }
}
