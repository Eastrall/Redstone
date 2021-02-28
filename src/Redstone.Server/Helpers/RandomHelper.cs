namespace Redstone.Server.Helpers
{
    internal static class RandomHelper
    {
        private static readonly int _entityObjectIdGenerator = 1;
        private static readonly object _entityObjectIdLock = new object();

        /// <summary>
        /// Generates an auto-incrementd entity object id.
        /// </summary>
        /// <returns>Auto incremented id.</returns>
        public static int GenerateEntityObjectId()
        {
            lock (_entityObjectIdLock)
            {
                return _entityObjectIdGenerator;
            }
        }
    }
}
