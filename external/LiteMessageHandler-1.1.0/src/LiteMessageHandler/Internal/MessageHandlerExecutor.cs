using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LiteMessageHandler.Internal;

internal class MessageHandlerExecutor
{
    private delegate void HandlerMethodExecuteAction(object target, object parameter);

    private static readonly MethodInfo? ExecuteMethod = typeof(IMessageHandler<>).GetMethod(nameof(IMessageHandler<object>.Execute));
    private readonly Type _handlerTargetType;
    private readonly Type _handlerParameterType;
    private readonly HandlerMethodExecuteAction _executor;

    public MessageHandlerExecutor(Type handlerTargetType, Type handlerParameterType)
    {
        _handlerTargetType = handlerTargetType ?? throw new ArgumentNullException(nameof(handlerTargetType));
        _handlerParameterType = handlerParameterType ?? throw new ArgumentNullException(nameof(handlerParameterType));
        _executor = Build() ?? throw new ArgumentException($"Failed to build message handler executor for the given type: '{_handlerTargetType.FullName}'");
    }

    public void Execute(object target, object parameter) => _executor(target, parameter);

    private HandlerMethodExecuteAction? Build()
    {
        MethodInfo? methodInfo = _handlerTargetType.GetMethod("Execute");

        if (methodInfo == null)
        {
            throw new ArgumentException($"Cannot find 'Execute' method in type: '{_handlerTargetType.FullName}'.");
        }

        ParameterExpression targetParameter = Expression.Parameter(typeof(object), "target");
        ParameterExpression methodParameter = Expression.Parameter(typeof(object), "parameter");

        UnaryExpression? instance = Expression.Convert(targetParameter, _handlerTargetType);
        UnaryExpression? parameter = Expression.Convert(methodParameter, _handlerParameterType);

        MethodCallExpression? methodCall = Expression.Call(instance, methodInfo, parameter);

        return Expression.Lambda<HandlerMethodExecuteAction>(methodCall, targetParameter, methodParameter).Compile();
    }
}