using Certes;
using Certes.Acme;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static FreeSSL.Domain.ISSLCtrlService;

namespace FreeSSL.Domain
{
	public interface ISSLCtrlService
	{
		Task<StartGetSSLResult> StartGetSSLAsync(string[] domains, AccountData accountData);

		Task<DownloadCersResult> TryDownloadCert(Guid id);

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

		public class DownloadCersResult
		{
			public string PrivateKey { get; set; }
			public string PemKey { get; set; }
			public DateTime ExpiredDate { get; set; }

			public DownloadCersResult() 
			{
				ExpiredDate = DateTime.UtcNow.AddDays(90);
			}
		}

		public class AccountData 
		{
			public string AccountPemKey { get; set; }
		}

	}

	public class SSLCtrlService : ISSLCtrlService
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly IMemoryCache _memCache;

		private string accountPemKey = null;

		public SSLCtrlService(IMemoryCache memoryCache, IHttpClientFactory factory)
		{
			_memCache = memoryCache;
			_clientFactory = factory;
		}

		public async Task<DownloadCersResult> TryDownloadCert(Guid id)
		{
			if (_memCache.TryGetValue<(IOrderContext, IChallengeContext[])>(id, out var result))
			{
				var (order, challenges) = result;

				await challenges.First().Validate();

				using var client = _clientFactory.CreateClient();

				var failedChecks = (await Task.WhenAll(challenges.Select(async (challenge) => {
					var result = await client.GetAsync(challenge.Location);
					return (challenge, challenge.KeyAuthz == await result.Content.ReadAsStringAsync());
				}))).Where(c => !c.Item2).Select(c => new FailedDomainValidation(c.challenge));

				if (failedChecks.Any())
					throw new HttpValidationException(failedChecks);

				foreach (var challenge in challenges)
				{
					await challenge.Validate();

					var validateResult = await challenge.Resource();

					if (validateResult.Status == Certes.Acme.Resource.ChallengeStatus.Invalid)
					{
						throw new Exception("Invalid validate error");
					}
				}

				var privateKey = KeyFactory.NewKey(KeyAlgorithm.RS256);

				await order.Finalize(new CsrInfo { }, privateKey);
				var certChain = await order.Download();

				return new DownloadCersResult
				{
					PemKey = certChain.Certificate.ToPem(),
					PrivateKey = privateKey.ToPem()
				};
			}

			throw new SessionExpirationException();
		}

		public async Task<StartGetSSLResult> StartGetSSLAsync(string[] domains, AccountData accountData)
		{
			domains = domains.Select(d => d.TrimEnd('/')).ToArray();
			
			AcmeContext acme;

			if (!string.IsNullOrEmpty(accountData?.AccountPemKey))
			{
				var accountKey = KeyFactory.FromPem(accountData?.AccountPemKey);
				acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2, accountKey);
			}
			else if (!string.IsNullOrEmpty(accountPemKey))
			{
				var accountKey = KeyFactory.FromPem(accountPemKey);
				acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2, accountKey);
			}
			else
			{
				acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2);
				var account = await acme.NewAccount("ulweader@gmail.com", true);
				accountPemKey = acme.AccountKey.ToPem();
			}

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
