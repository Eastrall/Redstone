using System;
using Microsoft.Extensions.DependencyInjection;

namespace Redstone.Common.DependencyInjection;

/// <summary>
/// Attribute that describes a class has a injectable service.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class InjectableAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the service life time.
    /// </summary>
    public ServiceLifetime LifeTime { get; set; }

    /// <summary>
    /// Creates a new <see cref="InjectableAttribute"/> instance.
    /// </summary>
    /// <param name="serviceLifeTime"></param>
    public InjectableAttribute(ServiceLifetime serviceLifeTime = ServiceLifetime.Transient)
    {
        LifeTime = serviceLifeTime;
    }
}
