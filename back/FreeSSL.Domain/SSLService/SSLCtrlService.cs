using Certes;
using Certes.Acme;
using FreeSSL.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static FreeSSL.Domain.SSLService.ISSLCtrlService;

namespace FreeSSL.Domain.SSLService
{
	public interface ISSLCtrlService
	{
		Task<StartGetSSLResult> StartGetSSLAsync(string[] domains, SSLAccountData accountData);

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

		public class SSLAccountData 
		{
			public string AccountPemKey { get; set; }
			public string Email { get; set; }
		}

	}

	public class SSLCtrlService : ISSLCtrlService
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly IMemoryCache _memCache;

		/// <summary>
		/// Used account that will be created after first use <see cref="StartGetSSLAsync(string[], SSLAccountData)"/>
		/// </summary>
		private string accountPemKey = null;

		public SSLCtrlService(IMemoryCache memoryCache, IHttpClientFactory factory)
		{
			_memCache = memoryCache;
			_clientFactory = factory;
		}

		public async Task<DownloadCersResult> TryDownloadCert(Guid id)
		{
			if (_memCache.TryGetValue<MakeSSLContext>(id, out var makeSSLCtx))
			{
				using var client = _clientFactory.CreateClient();

				var failedChecks = (await Task.WhenAll(makeSSLCtx.HttpChallengeResults.Select(async (challenge) =>
				{
					var result = await client.GetAsync(challenge.Location);
					return (challenge, challenge.Token == await result.Content.ReadAsStringAsync());
				}))).Where(c => !c.Item2).Select(c => new FailedDomainValidation(c.challenge));

				if (failedChecks.Any())
					throw new HttpValidationException(failedChecks);

				foreach (var challenge in makeSSLCtx.Challenges)
				{
					await challenge.Validate();

					var validateResult = await challenge.Resource();

					if (validateResult.Status == Certes.Acme.Resource.ChallengeStatus.Invalid)
					{
						throw new Exception("Invalid validate error");
					}
				}

				var privateKey = KeyFactory.NewKey(KeyAlgorithm.RS256);

				var certChain = await makeSSLCtx.OrderCtrx.Generate(new CsrInfo {}, privateKey);

				return new DownloadCersResult
				{
					PemKey = certChain.Certificate.ToPem(),
					PrivateKey = privateKey.ToPem()
				};
			}

			throw new SessionExpirationException();
		}

		public async Task<StartGetSSLResult> StartGetSSLAsync(string[] domains, SSLAccountData accountData)
		{
			domains = domains.Select(d => d.TrimEnd('/')).ToArray();
			
			AcmeContext acme;

			if (!string.IsNullOrEmpty(accountData?.AccountPemKey))
			{
				var accountKey = KeyFactory.FromPem(accountData?.AccountPemKey);
				acme = new AcmeContext(WellKnownServers.LetsEncryptV2, accountKey);
			}
			else if (!string.IsNullOrEmpty(accountPemKey))
			{
				var accountKey = KeyFactory.FromPem(accountPemKey);
				acme = new AcmeContext(WellKnownServers.LetsEncryptV2, accountKey);
			}
			else
			{
				acme = new AcmeContext(WellKnownServers.LetsEncryptV2);
				var account = await acme.NewAccount(accountData?.Email, true);
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

			_memCache.Set(getSSLResults.Id, new MakeSSLContext(order, challenges, getSSLResults.ChalengeResults), new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
			});

			return getSSLResults;

		}

	}
}
