using System;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Extensions
{
    /// <summary>
    /// Provides extensions to the <see cref="ParameterInfo"/> class.
    /// </summary>
    internal static class ParameterInfoExtensions
    {
        /// <summary>
        /// Gets the default value of a <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter">Parameter informations.</param>
        /// <returns>Default value.</returns>
        public static object GetParameterDefaultValue(this ParameterInfo parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.HasDefaultValue)
            {
                return parameter.DefaultValue;
            }

            return parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null;
        }
    }
}
