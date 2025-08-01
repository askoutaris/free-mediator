using System.Threading;
using System.Threading.Tasks;

namespace FreeMediator
{
	public interface IBaseRequest { }

	public interface IRequest : IBaseRequest { }

	public interface IRequest<out TResponse> : IBaseRequest { }

	public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}
}
