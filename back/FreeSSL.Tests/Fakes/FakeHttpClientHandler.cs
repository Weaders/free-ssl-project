using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FreeSSL.Tests.Fakes
{
	public class FakeHttpClientHandler : HttpClientHandler
	{
		protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			throw new Exception("Can not send http on tests");
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			throw new Exception("Can not send http on tests");
		}

	}
}
