using System;
using System.Diagnostics.CodeAnalysis;

namespace Redstone.Common.Utilities;

/// <summary>
/// Provides utility methods for the <see cref="DateTime"/> type.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TimeUtilities
{
    private static readonly DateTime _initialTime;

    static TimeUtilities()
    {
        _initialTime = new DateTime(1970, 1, 1);
    }

    /// <summary>
    /// Gets the elapsed time since 1/1/1970.
    /// </summary>
    /// <returns>Elapsed time span since 1/1/1970.</returns>
    public static TimeSpan GetElapsedTimeSpan() => DateTime.UtcNow - _initialTime;

    /// <summary>
    /// Gets the elapsed time since 1/1/1970 in milliseconds.
    /// </summary>
    /// <returns>Elapsed time in milliseconds since 1/1/1970.</returns>
    public static long GetElapsedMilliseconds() => (long)GetElapsedTimeSpan().TotalMilliseconds;
}
