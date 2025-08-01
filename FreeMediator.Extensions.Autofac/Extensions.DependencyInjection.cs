using System.Reflection;
using Autofac;

namespace FreeMediator.Extensions.Autofac
{
	public static class DependencyInjectionExtensions
	{
		public static void AddMediator(this ContainerBuilder builder, params Assembly[] assemblies)
		{
			builder
				.RegisterAssemblyTypes(assemblies)
				.AsClosedTypesOf(typeof(IRequestHandler<,>))
				.InstancePerLifetimeScope();

			builder
				.RegisterType<Mediator>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder
				.RegisterType<HandlerFactory>()
				.AsImplementedInterfaces()
				.SingleInstance();
		}
	}
}
