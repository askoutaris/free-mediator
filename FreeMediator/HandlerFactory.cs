using System;
using System.Collections.Concurrent;

namespace FreeMediator
{
	public interface IHandlerFactory
	{
		IHandlerInvoker Create<TResponse>(IRequest<TResponse> request);
	}

	public class HandlerFactory : IHandlerFactory
	{
		private readonly ConcurrentDictionary<Type, IHandlerInvoker> _handlers;

		public HandlerFactory()
		{
			_handlers = [];
		}

		public IHandlerInvoker Create<TResponse>(IRequest<TResponse> request)
			=> _handlers.GetOrAdd(request.GetType(), type => CreateHandler(request));

		private HandlerInvoker CreateHandler<TResponse>(IRequest<TResponse> request)
		{
			var requestType = request.GetType();

			var responseType = typeof(TResponse);

			var handlerType = (typeof(IRequestHandler<,>)).MakeGenericType(requestType, responseType);

			var methodName = nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle);

			var method = handlerType.GetMethod(methodName) ?? throw new Exception($"No {methodName} method found in type {handlerType}");

			return new HandlerInvoker(handlerType, method);
		}
	}
}
