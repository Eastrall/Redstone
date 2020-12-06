namespace Redstone.Common.Configuration
{
    /// <summary>
    /// Provides a data structure to configure the Redstone server game settings.
    /// </summary>
    public class GameConfiguration
    {
        /// <summary>
        /// Gets or sets the server game mode.
        /// </summary>
        public ServerGameModeType Mode { get; set; }

        /// <summary>
        /// Gets or sets the server map seed used during terrain generation.
        /// </summary>
        public long Seed { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the respawn screen should be displayed in case of player's death.
        /// </summary>
        public bool DisplayRespawnScreen { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the game is running in hardcore mode.
        /// </summary>
        public bool IsHardcore { get; set; }

        /// <summary>
        /// Gets or sets the chunk rendering distance.
        /// </summary>
        /// <remarks>
        /// This value is ranged between 2 and 32.
        /// </remarks>
        public int RenderingDistance { get; set; }
    }
}
