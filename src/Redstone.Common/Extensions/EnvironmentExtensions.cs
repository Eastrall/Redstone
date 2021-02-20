using System;

namespace Redstone.Common.Extensions
{
    public static class EnvironmentExtensions
    {
        private static readonly string _dotnetRunningInDockerKey = "DOTNET_RUNNING_IN_CONTAINER";
        private static readonly string _redstoneEnvironmentPathKey = "REDSTONE_PATH";
        private static readonly string _redstoneDefaultDockerPath = "/opt/redstone";

        /// <summary>
        /// Checks if the program is running inside a docker container.
        /// </summary>
        /// <returns>True if running inside docker; false otherwise.</returns>
        public static bool IsRunningInDocker()
        {
            return bool.TryParse(Environment.GetEnvironmentVariable(_dotnetRunningInDockerKey), out bool result) && result;
        }

        /// <summary>
        /// Gets the current environment directory where the program is actually executing.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentEnvironementDirectory()
        {
            if (IsRunningInDocker())
            {
                return Environment.GetEnvironmentVariable(_redstoneEnvironmentPathKey) ?? _redstoneDefaultDockerPath;
            }

            return Environment.CurrentDirectory;
        }
    }
}
