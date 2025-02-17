namespace MinesweeperApi.Application.Models
{
    using AutoMapper;
    using MinesweeperApi.Infrastructure.Data.Entities;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Represents the game model containing all necessary game configuration and state details.
    /// </summary>
    public class GameModel : IGameModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the game.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the width of the game board.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the game board.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the total number of mines placed on the game board.
        /// </summary>
        public int MinesCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game has been completed.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the current state of the game board represented as a two-dimensional array.
        /// </summary>
        public int[,] CurrentField { get; set; }
    }

    /// <summary>
    /// Custom value resolver that converts a <see cref="GameModel"/> instance into its JSON string representation,
    /// suitable for storing in a <see cref="GameEntity"/>.
    /// </summary>
    public class GameModelToGameJsonResolver : IValueResolver<GameModel, GameEntity, string>
    {
        /// <summary>
        /// Resolves the destination member by serializing the source <see cref="GameModel"/> into a JSON string.
        /// </summary>
        /// <param name="source">The source <see cref="GameModel"/> instance.</param>
        /// <param name="destination">The destination <see cref="GameEntity"/> instance (not used in this resolver).</param>
        /// <param name="destMember">The destination member value (ignored in this implementation).</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>A JSON string representation of the <see cref="GameModel"/>.</returns>
        public string Resolve(GameModel source, GameEntity destination, string destMember, ResolutionContext context)
        {
            // Serialize the GameModel instance to a JSON string.
            var json = JsonConvert.SerializeObject(source);
            return json;
        }
    }

    /// <summary>
    /// Configures mappings between <see cref="GameEntity"/> and <see cref="GameModel"/> using AutoMapper.
    /// </summary>
    public class GameProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameProfile"/> class and sets up mapping configurations.
        /// </summary>
        public GameProfile()
        {
            // Mapping from GameEntity to GameModel.
            CreateMap<GameEntity, GameModel>()
                // Convert the string Id from GameEntity to a Guid for GameModel.
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                // After mapping, deserialize the JSON stored in GameEntity.Game into a temporary GameModel,
                // then assign individual properties to the destination GameModel.
                .AfterMap((src, dest) =>
                {
                    var tmp = JsonConvert.DeserializeObject<GameModel>(src.Game);

                    dest.Width = tmp.Width;
                    dest.Height = tmp.Height;
                    dest.MinesCount = tmp.MinesCount;
                    dest.Completed = tmp.Completed;
                    dest.CurrentField = tmp.CurrentField;
                });

            // Mapping from GameModel to GameEntity.
            CreateMap<GameModel, GameEntity>()
                // Convert the Guid Id from GameModel to a string for GameEntity.
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                // Convert the entire GameModel into a JSON string for storage in GameEntity.Game using a custom resolver.
                .ForMember(dest => dest.Game, opt => opt.MapFrom<GameModelToGameJsonResolver>());
        }
    }
}
