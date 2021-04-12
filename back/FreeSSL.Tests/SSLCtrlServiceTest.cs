using FreeSSL.Domain;
using FreeSSL.Tests.Core;
using FreeSSL.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace FreeSSL.Tests
{
	public class SSLCtrlServiceTest
	{
		[Fact]
		public async void NoSession()
		{
			var collection = new TestServiceCollection();

			collection.AddSingleton(new MockHttpClientFactory().Object);

			collection.AddMemoryCache();

			var serviceProvider = collection.GetServiceProvider();

			var ctrl = serviceProvider.CallConstructorWithDI<SSLCtrlService>(typeof(SSLCtrlService).GetConstructors()[0]);

			await Assert.ThrowsAsync<SessionExpirationException>(() => ctrl.TryDownloadCert(Guid.NewGuid()));

		}
	}
}
