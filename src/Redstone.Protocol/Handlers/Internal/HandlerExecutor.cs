using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Redstone.Protocol.Handlers.Internal;

internal sealed class HandlerExecutor
{
    private delegate object HandlerMethodExecutor(object target, object[] parameters);
    private delegate void VoidHandlerMethodExecutor(object target, object[] parameters);

    private readonly TypeInfo _targetHandlerTypeInfo;
    private readonly MethodInfo _handlerActionMethodInfo;
    private readonly object[] _handlerActionDefaultParameters;
    private readonly HandlerMethodExecutor _executor;

    public ParameterInfo[] MethodParameters { get; }

    public HandlerExecutor(TypeInfo targetHandlerTypeInfo, MethodInfo handlerActionMethodInfo, object[] defaultParameters)
    {
        _targetHandlerTypeInfo = targetHandlerTypeInfo;
        _handlerActionMethodInfo = handlerActionMethodInfo;
        _handlerActionDefaultParameters = defaultParameters;
        _executor = BuildExecutor();

        MethodParameters = handlerActionMethodInfo.GetParameters();
    }

    public object Execute(object target, params object[] parameters) => _executor(target, parameters);

    private HandlerMethodExecutor BuildExecutor()
    {
        ParameterExpression targetParameter = Expression.Parameter(typeof(object), "target");
        ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

        var parameters = new List<Expression>();
        ParameterInfo[] parameterInfos = _handlerActionMethodInfo.GetParameters();

        for (var i = 0; i < parameterInfos.Length; i++)
        {
            ParameterInfo paramInfo = parameterInfos[i];
            BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
            UnaryExpression valueCast = Expression.Convert(valueObj, paramInfo.ParameterType);

            parameters.Add(valueCast);
        }

        UnaryExpression instanceCast = Expression.Convert(targetParameter, _targetHandlerTypeInfo.AsType());
        MethodCallExpression methodCall = Expression.Call(instanceCast, _handlerActionMethodInfo, parameters);

        if (methodCall.Type == typeof(void))
        {
            var executor = Expression.Lambda<VoidHandlerMethodExecutor>(methodCall, targetParameter, parametersParameter).Compile();

            return (target, args) =>
            {
                executor(target, args);
                return null;
            };
        }
        else
        {
            UnaryExpression castMethodCall = Expression.Convert(methodCall, typeof(object));

            return Expression.Lambda<HandlerMethodExecutor>(castMethodCall, targetParameter, parametersParameter).Compile();
        }
    }

    public object GetDefaultValueForParameter(int index)
    {
        if (index < 0 || index > MethodParameters.Length - 1)
            throw new ArgumentOutOfRangeException(nameof(index));

        return _handlerActionDefaultParameters[index];
    }
}
