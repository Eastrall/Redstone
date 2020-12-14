using Redstone.Protocol.Handlers.Internal.Transformers;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Models
{
    internal class TransformerModel
    {
        /// <summary>
        /// Gets the source parameter information to be transformed.
        /// </summary>
        public TypeInfo Source { get; }

        /// <summary>
        /// Gets the destiantion parameter informations.
        /// </summary>
        public TypeInfo Destination { get; }

        /// <summary>
        /// Gets the transformation parameter fucntion.
        /// </summary>
        public TransformerFuntion Transformer { get; }

        /// <summary>
        /// Creates a new <see cref="TransformerModel"/> instance.
        /// </summary>
        /// <param name="source">Source parameter information.</param>
        /// <param name="destination">Destination parameter information.</param>
        /// <param name="transformer">Transformer function.</param>
        public TransformerModel(TypeInfo source, TypeInfo destination, TransformerFuntion transformer)
        {
            Source = source;
            Destination = destination;
            Transformer = transformer;
        }
    }
}
