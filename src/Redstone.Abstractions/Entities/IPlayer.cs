namespace Redstone.Abstractions.Entities
{
    /// <summary>
    /// Provides an interface that describes the player behavior.
    /// </summary>
    public interface IPlayer : IEntity
    {
        /// <summary>
        /// Gets the player name.
        /// </summary>
        string Name { get; }
    }
}
