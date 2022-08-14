using System;

namespace Redstone.Common.Structures.Blocks;

/// <summary>
/// Provides an attribute to tag a class with a minecraft block name.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class BlockNameAttribute : Attribute
{
    /// <summary>
    /// Gets the block name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Creates a new <see cref="BlockNameAttribute"/> instance.
    /// </summary>
    /// <param name="name">Block name. (Eg. minecraft_air)</param>
    public BlockNameAttribute(string name)
    {
        Name = name;
    }
}
