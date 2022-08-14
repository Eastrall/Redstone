using System;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Extensions;

/// <summary>
/// Provides extensions to the <see cref="MethodInfo"/> class.
/// </summary>
internal static class MethodInfoExtensions
{
    /// <summary>
    /// Gets a <see cref="MethodInfo"/> parameters default values.
    /// </summary>
    /// <param name="methodInfo">Method informations.</param>
    /// <returns>Array of the default method parameters.</returns>
    public static object[] GetMethodParametersDefaultValues(this MethodInfo methodInfo)
    {
        if (methodInfo is null)
        {
            throw new ArgumentNullException(nameof(methodInfo));
        }

        ParameterInfo[] methodParameters = methodInfo.GetParameters();
        var methodParametersValues = new object[methodParameters.Length];

        for (var i = 0; i < methodParametersValues.Length; i++)
        {
            methodParametersValues[i] = methodParameters[i]?.GetParameterDefaultValue();
        }

        return methodParametersValues;
    }
}
