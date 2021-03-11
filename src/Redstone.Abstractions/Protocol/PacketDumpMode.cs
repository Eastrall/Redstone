namespace Redstone.Abstractions.Protocol
{
    public enum PacketDumpMode
    {
        /// <summary>
        /// Outputs the packet buffer as one byte per line.
        /// </summary>
        Default,

        /// <summary>
        /// Ouputs the packet buffer as a UTF8 string.
        /// </summary>
        UTF8String
    }
}
