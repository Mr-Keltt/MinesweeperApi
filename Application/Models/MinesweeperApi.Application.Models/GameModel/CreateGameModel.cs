using AutoMapper;
using MinesweeperApi.Infrastructure.Data.Entities;
using Newtonsoft.Json;

namespace MinesweeperApi.Application.Models
{
    /// <summary>
    /// Represents the model for creating a new game with the required configuration details.
    /// </summary>
    public class CreateGameModel : IGameModel
    {
        /// <summary>
        /// Gets or sets the width of the game board.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game board.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the number of mines to be placed on the game board.
        /// </summary>
        public int MinesCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is completed.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the current state of the game board as a two-dimensional array.
        /// </summary>
        public int[,] CurrentField { get; set; }
    }

    /// <summary>
    /// Provides AutoMapper profile configuration for mapping a <see cref="CreateGameModel"/> to a <see cref="GameEntity"/>.
    /// </summary>
    public class CreateGameProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGameProfile"/> class.
        /// Configures mapping rules from <see cref="CreateGameModel"/> to <see cref="GameEntity"/>.
        /// </summary>
        public CreateGameProfile()
        {
            CreateMap<CreateGameModel, GameEntity>()
                // Generate a new unique identifier for the GameEntity.Id property.
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                // Map the CreateGameModel to a JSON string for the GameEntity.Game property using a custom resolver.
                .ForMember(dest => dest.Game, opt => opt.MapFrom<CreateGameModelToGameJsonResolver>());
        }
    }

    /// <summary>
    /// Custom value resolver that converts a <see cref="CreateGameModel"/> instance into its JSON string representation.
    /// </summary>
    public class CreateGameModelToGameJsonResolver : IValueResolver<CreateGameModel, GameEntity, string>
    {
        /// <summary>
        /// Resolves the destination member by serializing the source <see cref="CreateGameModel"/> into a JSON string.
        /// </summary>
        /// <param name="source">The source <see cref="CreateGameModel"/> instance.</param>
        /// <param name="destination">The destination <see cref="GameEntity"/> instance (unused in this resolver).</param>
        /// <param name="destMember">The destination member value (ignored in this implementation).</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>A JSON string representing the <see cref="CreateGameModel"/>.</returns>
        public string Resolve(CreateGameModel source, GameEntity destination, string destMember, ResolutionContext context)
        {
            // Serialize the CreateGameModel instance to a JSON string.
            var json = JsonConvert.SerializeObject(source);
            return json;
        }
    }
}
