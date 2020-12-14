using Microsoft.Extensions.DependencyInjection;
using Redstone.Protocol.Handlers.Internal;
using Redstone.Protocol.Handlers.Internal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redstone.Protocol.Handlers
{
    /// <summary>
    /// Helper class used to load the packet handlers.
    /// </summary>
    internal static class PacketHandlerLoader
    {
        /// <summary>
        /// Loads the redstone packet handlers.
        /// </summary>
        /// <returns></returns>
        public static IDictionary<MinecraftUserStatus, IHandlerActionCache> LoadHandlers(IEnumerable<Assembly> assemblies)
        {
            var caches = new Dictionary<MinecraftUserStatus, IHandlerActionCache>();
            var minecraftStatus = Enum.GetValues(typeof(MinecraftUserStatus)).Cast<MinecraftUserStatus>();

            foreach (MinecraftUserStatus status in minecraftStatus)
            {
                caches.Add(status, GetCache(status, assemblies));
            }

            return caches;
        }

        private static IHandlerActionCache GetCache(MinecraftUserStatus status, IEnumerable<Assembly> assemblies)
        {
            var handlerCacheEntries = new Dictionary<object, HandlerActionModel>();
            IEnumerable<Type> handlerTypes = from x in assemblies.SelectMany(a => a.GetTypes())
                                             where x.GetMethods().Any(m => m.GetCustomAttributes(GetPacketHandlerAttributeType(status)).Any())
                                             select x;

            foreach (Type handlerType in handlerTypes)
            {
                TypeInfo handlerTypeInfo = handlerType.GetTypeInfo();
                IEnumerable<HandlerActionModel> handlerActions = from x in handlerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                                                 let attribute = x.GetCustomAttribute(GetPacketHandlerAttributeType(status)) as PacketHandlerAttribute
                                                                 where attribute != null
                                                                 select new HandlerActionModel(attribute.Action, x, handlerTypeInfo);

                foreach (HandlerActionModel handlerAction in handlerActions)
                {
                    if (!handlerCacheEntries.ContainsKey(handlerAction.ActionType))
                    {
                        handlerCacheEntries.Add(handlerAction.ActionType, handlerAction);
                    }
                }
            }

            return new HandlerActionCache(handlerCacheEntries);
        }

        private static Type GetPacketHandlerAttributeType(MinecraftUserStatus status)
        {
            return status switch
            {
                MinecraftUserStatus.Handshaking => typeof(HandshakePacketHandlerAttribute),
                MinecraftUserStatus.Status => typeof(StatusPacketHandlerAttribute),
                MinecraftUserStatus.Login => typeof(LoginPacketHandlerAttribute),
                MinecraftUserStatus.Play => typeof(PlayPacketHandlerAttribute),
                _ => throw new NotImplementedException()
            };
        }
    }
}
