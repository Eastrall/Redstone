namespace Redstone.CLI.Registries.Converters
{
    /// <summary>
    /// Provides a mechanism to convert an input source into a string output.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Converts the input to a string output.
        /// </summary>
        /// <returns></returns>
        string Convert();
    }
}
