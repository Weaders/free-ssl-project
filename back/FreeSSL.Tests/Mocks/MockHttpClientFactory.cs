using FreeSSL.Tests.Fakes;
using Moq;
using System.Net.Http;

namespace FreeSSL.Tests.Mocks
{
	public class MockHttpClientFactory : Mock<IHttpClientFactory>
	{
		public MockHttpClientFactory()
		{
			Setup(c => c.CreateClient(It.IsAny<string>())).Returns(() => new HttpClient(new FakeHttpClientHandler()));
		}

	}
}
