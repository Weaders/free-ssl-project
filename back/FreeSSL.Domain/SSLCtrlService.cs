using Certes;
using Certes.Acme;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FreeSSL.Domain.ISSLCtrlService;

namespace FreeSSL.Domain
{
	public interface ISSLCtrlService
	{
		Task<StartGetSSLResult> StartGetSSLAsync(string[] domains);

		Task<string> TryDownloadCert(Guid id);

		public class StartGetSSLResult
		{
			public StartGetSSLResult()
			{
				Id = Guid.NewGuid();
			}

			public Guid Id { get; set; }
			public List<HttpChallengeResult> ChalengeResults { get; set; } = new List<HttpChallengeResult>();
		}

		public class HttpChallengeResult
		{
			public string Key { get; set; }
			public string Token { get; set; }
			public string Location { get; set; }
		}

	}

	public class SSLCtrlService : ISSLCtrlService
	{

		private readonly IMemoryCache _memCache;

		public SSLCtrlService(IMemoryCache memoryCache)
		{
			_memCache = memoryCache;
		}

		public async Task<string> TryDownloadCert(Guid id)
		{
			if (_memCache.TryGetValue<(IOrderContext, IChallengeContext[])>(id, out var result))
			{
				var (order, challenges) = result;

				foreach (var challenge in challenges)
				{
					var validateResult = await challenge.Validate();
					if (validateResult.Status == Certes.Acme.Resource.ChallengeStatus.Invalid)
					{
						throw new Exception("Invalid validate error");
					}
				}

				var cert = await order.Download();
				return cert.ToPem();

			}

			throw new Exception("Wait for too long, you session expired");
		}

		public async Task<StartGetSSLResult> StartGetSSLAsync(string[] domains)
		{
			domains = domains.Select(d => d.TrimEnd('/')).ToArray();

			var acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2);
			var acc = acme.NewAccount("ulweader@gmail.com", true);

			//var accKey = acme.AccountKey.ToPem();

			var order = await acme.NewOrder(domains);
			var challenges = await Task.WhenAll((await order.Authorizations()).Select(a => a.Http()));

			var enumDomain = domains.GetEnumerator();

			var getSSLResults = new StartGetSSLResult
			{
				ChalengeResults = challenges.Select(c =>
				{
					enumDomain.MoveNext();

					return new HttpChallengeResult
					{
						Key = c.KeyAuthz,
						Location = $"http://{enumDomain.Current}/.well-known/acme-challenge/{c.Token}",
						Token = c.Token
					};

				}).ToList()
			};

			_memCache.Set(getSSLResults.Id, (order, challenges), new MemoryCacheEntryOptions
			{ 
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
			});

			return getSSLResults;

		}

	}
}
