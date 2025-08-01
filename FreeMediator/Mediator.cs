using System;
using System.Threading;
using System.Threading.Tasks;

namespace FreeMediator
{
	public interface IMediator
	{
		Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
		Task<object?> Send(object request, CancellationToken ct = default);
	}

	public class Mediator : IMediator
	{
		private readonly IServiceProvider _services;
		private readonly IHandlerFactory _factory;

		public Mediator(IServiceProvider services, IHandlerFactory factory)
		{
			_services = services;
			_factory = factory;
		}

		public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
		{
			var handler = _factory.Create(request);

			return handler.Handle(_services, request, ct);
		}

		public Task<object?> Send(object request, CancellationToken ct = default)
		{
			var handler = _factory.Create(request);

			return handler.Handle(_services, request, ct);
		}
	}
}
