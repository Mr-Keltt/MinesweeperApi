namespace MinesweeperApi.Infrastructure.Data.Entities
{
    /// <summary>
    /// Represents a game entity with a unique identifier and associated game data.
    /// </summary>
    public class GameEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the game entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the game data in a serialized or string format.
        /// </summary>
        public string Game { get; set; }
    }
}
