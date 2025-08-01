using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FreeMediator
{
	public interface IHandlerInvoker
	{
		Task<TResponse> Handle<TResponse>(IServiceProvider services, IRequest<TResponse> request, CancellationToken ct = default);
		Task<object?> Handle(IServiceProvider services, object request, CancellationToken ct = default);
	}

	class HandlerInvoker : IHandlerInvoker
	{
		private readonly Type _handlerType;
		private readonly MethodInfo _method;

		public HandlerInvoker(Type handlerType, MethodInfo method)
		{
			_handlerType = handlerType;
			_method = method;
		}

		public async Task<TResponse> Handle<TResponse>(IServiceProvider services, IRequest<TResponse> request, CancellationToken ct = default)
		{
			var handler = services.GetRequiredService(_handlerType);

			var result = _method.Invoke(handler, [request, ct]) ?? throw new Exception($"Method {_method.Name} in type {_handlerType} returned null which was not expected");

			return await (Task<TResponse>)result;
		}

		public async Task<object?> Handle(IServiceProvider services, object request, CancellationToken ct = default)
		{
			var handler = services.GetRequiredService(_handlerType);

			dynamic result = _method.Invoke(handler, [request, ct]) ?? throw new Exception($"Method {_method.Name} in type {_handlerType} returned null which was not expected");

			object? taskResult = await result;

			return taskResult;
		}
	}
}
