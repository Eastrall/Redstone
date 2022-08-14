using Redstone.Abstractions.Protocol;
using Redstone.Protocol.Handlers.Exceptions;
using Redstone.Protocol.Handlers.Internal;
using Redstone.Protocol.Handlers.Internal.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redstone.Protocol.Handlers;

internal class PacketHandlerInvoker : IPacketHandler
{
    private readonly IParameterTransformer _parameterTransformer = null!;
    private readonly IDictionary<MinecraftUserStatus, HandlerActionInvokerCache> _invokerCache;

    public PacketHandlerInvoker(IDictionary<MinecraftUserStatus, HandlerActionInvokerCache> invokerCache)
    {
        _invokerCache = invokerCache;
    }

    public object Invoke(MinecraftUserStatus status, object handler, params object[] parameters)
    {
        if (!_invokerCache.TryGetValue(status, out HandlerActionInvokerCache cache))
        {
            throw new InvalidOperationException($"Failed to find invoker cache for status: '{status}'.");
        }

        HandlerActionInvokerCacheEntry handlerActionInvoker = cache.GetCachedHandlerAction(handler);

        if (handlerActionInvoker is null)
        {
            throw new HandlerActionNotFoundException(handler);
        }

        var targetHandler = handlerActionInvoker.HandlerFactory(handlerActionInvoker.HandlerType);

        if (targetHandler is null)
        {
            throw new HandlerTargetCreationFailedException(handlerActionInvoker.HandlerType);
        }

        object handlerResult = null;

        try
        {
            object[] handlerActionParameters = PrepareParameters(parameters, handlerActionInvoker.HandlerExecutor);

            handlerResult = handlerActionInvoker.HandlerExecutor.Execute(targetHandler, handlerActionParameters);
        }
        catch
        {
            throw;
        }
        finally
        {
            handlerActionInvoker.HandlerReleaser(targetHandler);
        }

        return handlerResult;
    }

    /// <summary>
    /// Prepare the invoke parameters. Adds default values if a parameter is missing.
    /// </summary>
    /// <param name="originalParameters">Original parameters.</param>
    /// <param name="executor">Handler executor.</param>
    /// <returns>Handler parameters.</returns>
    private object[] PrepareParameters(object[] originalParameters, HandlerExecutor executor)
    {
        if (!executor.MethodParameters.Any())
        {
            return null;
        }

        var parameters = new object[executor.MethodParameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            ParameterInfo methodParameterInfo = executor.MethodParameters.ElementAt(i);

            if (i < originalParameters.Length)
            {
                Type originalObjectType = originalParameters[i]?.GetType();

                if (!methodParameterInfo.ParameterType.IsAssignableFrom(originalObjectType))
                {
                    object transformedParameter = _parameterTransformer?.Transform(originalParameters[i], methodParameterInfo.ParameterType.GetTypeInfo());

                    parameters[i] = transformedParameter ?? executor.GetDefaultValueForParameter(i);
                }
                else
                {
                    parameters[i] = originalParameters[i];
                }
            }
            else
            {
                parameters[i] = executor.GetDefaultValueForParameter(i);
            }
        }

        return parameters;
    }
}
