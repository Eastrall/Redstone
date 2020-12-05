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
    }
}
