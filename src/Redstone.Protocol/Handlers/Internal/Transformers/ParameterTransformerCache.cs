using Redstone.Protocol.Handlers.Internal.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal.Transformers;

internal sealed class ParameterTransformerCache
{
    private readonly IDictionary<TypeInfo, ParameterTransformerCacheEntry> _cache;
    private readonly IDictionary<TypeInfo, TransformerModel> _transformers;
    private readonly IParameterFactory _parameterFactory;

    /// <summary>
    /// Creates a new <see cref="ParameterTransformerCache"/> instance.
    /// </summary>
    /// <param name="parameterFactory">Parameter factory.</param>
    public ParameterTransformerCache(IParameterFactory parameterFactory)
    {
        _cache = new ConcurrentDictionary<TypeInfo, ParameterTransformerCacheEntry>();
        _transformers = new ConcurrentDictionary<TypeInfo, TransformerModel>();
        _parameterFactory = parameterFactory;
    }

    /// <summary>
    /// Adds a new <see cref="TransformerModel"/> to the inner cache.
    /// </summary>
    /// <param name="transformerModel"></param>
    public void AddTransformer(TransformerModel transformerModel)
    {
        _transformers.Add(transformerModel.Destination, transformerModel);
    }

    /// <summary>
    /// Gets a transformer by its type info key.
    /// </summary>
    /// <param name="type">TypeInfo key.</param>
    /// <returns>
    /// Existing <see cref="ParameterTransformerCacheEntry"/>; otherwise the system creates it and caches it.
    /// </returns>
    public ParameterTransformerCacheEntry GetTransformer(TypeInfo type)
    {
        if (!_cache.TryGetValue(type, out ParameterTransformerCacheEntry transformer))
        {
            TransformerModel transformerModel = GetTransformerModel(type);

            if (transformerModel == null)
            {
                return null;
            }

            transformer = new ParameterTransformerCacheEntry(
                transformerModel.Source, 
                transformerModel.Destination, 
                type,
                _parameterFactory.Create,
                transformerModel.Transformer);

            _cache.Add(type, transformer);
        }

        return transformer;
    }

    /// <summary>
    /// Gets the best transformer model matching the type info.
    /// </summary>
    /// <param name="type">Type Informations.</param>
    /// <returns>Best transformer model.</returns>
    private TransformerModel GetTransformerModel(TypeInfo type)
    {
        if (!_transformers.TryGetValue(type, out TransformerModel transformer))
        {
            TypeInfo[] interfaces = type.GetInterfaces().Select(x => x.GetTypeInfo()).ToArray();

            for (int i = 0; i < interfaces.Length; i++)
            {
                if (_transformers.TryGetValue(interfaces[i], out transformer))
                {
                    break;
                }
            }
        }

        return transformer;
    }
}
