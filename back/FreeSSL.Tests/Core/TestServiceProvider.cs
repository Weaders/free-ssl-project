using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Linq;
using System.Reflection;

namespace FreeSSL.Tests.Core
{
	public class TestServiceCollection : ServiceCollection
	{
		public TestServiceCollection()
		{
			var testEnv = new Mock<IHostEnvironment>();

			testEnv.Setup(t => t.EnvironmentName).Returns("Development");

			this.AddSingleton(testEnv.Object);
		}

		public ITestServiceProvider GetServiceProvider()
			=> new TestServiceProvider(new DefaultServiceProviderFactory().CreateServiceProvider(this));

	}

	public interface ITestServiceProvider : IServiceProvider
	{
		T CallConstructorWithDI<T>(ConstructorInfo constructorInfo)
			where T : class;
	}

	public class TestServiceProvider : IServiceProvider, ITestServiceProvider
	{
		private IServiceProvider _serviceProvider;

		public TestServiceProvider(IServiceProvider serviceProvider)
			=> _serviceProvider = serviceProvider;

		public T CallConstructorWithDI<T>(ConstructorInfo constructorInfo)
			where T : class
		{
			var parameters = constructorInfo.GetParameters()
				.Select(p => GetService(p.ParameterType))
				.ToArray();

			return Activator.CreateInstance(typeof(T), parameters) as T;
		}

		public object GetService(Type serviceType)
			=> _serviceProvider.GetService(serviceType);

	}
}
