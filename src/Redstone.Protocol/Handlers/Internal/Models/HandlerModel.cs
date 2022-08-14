using System.Collections.Generic;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Models;

/// <summary>
/// Provides a high level structure for a handler.
/// </summary>
internal class HandlerModel
{
    /// <summary>
    /// Gets the handler name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the handler type informations.
    /// </summary>
    public TypeInfo TypeInfo { get; }

    /// <summary>
    /// Gets the handler actions associated to this handler.
    /// </summary>
    public IList<HandlerActionModel> Actions { get; }

    /// <summary>
    /// Creates a new <see cref="HandlerModel"/> instance.
    /// </summary>
    /// <param name="handlerType">Handler type information.</param>
    /// <param name="actions">Handler actions</param>
    public HandlerModel(TypeInfo handlerType, IEnumerable<HandlerActionModel> actions)
    {
        Name = handlerType.Name;
        TypeInfo = handlerType;
        Actions = new List<HandlerActionModel>(actions);
    }
}
