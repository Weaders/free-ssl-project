using Certes;
using Certes.Acme;
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

		Task<bool> CheckHttpResultAsync(HttpChallengeResult challenge);

		public class StartGetSSLResult
		{
			public List<HttpChallengeResult> ChalengeResults = new List<HttpChallengeResult>();
		}

		public class HttpChallengeResult
		{
			public string Key { get; set; }
			public string Token { get; set; }
			public Uri Location { get; set; }
		}


	}

	public class SSLCtrlService : ISSLCtrlService
	{
		public Task<bool> CheckHttpResultAsync(HttpChallengeResult challenge)
		{
			throw new NotImplementedException();
		}

		//public SSLCtrlService() { }

		//public Task<bool> CheckHttpResultAsync(HttpChallengeResult challenge)
		//{
		//	var using req = 
		//}

		public async Task<StartGetSSLResult> StartGetSSLAsync(string[] domains)
		{
			var acme = new AcmeContext(WellKnownServers.LetsEncryptStagingV2);
			var acc = acme.NewAccount("ulweader@gmail.com", true);

			//var accKey = acme.AccountKey.ToPem();

			var order = await acme.NewOrder(domains);
			var challenges = await Task.WhenAll((await order.Authorizations()).Select(a => a.Http()));

			var getSSLResults = new StartGetSSLResult();

			getSSLResults.ChalengeResults = challenges.Select(c => new HttpChallengeResult
			{
				Key = c.KeyAuthz,
				Location = c.Location,
				Token = c.Token
			}).ToList();

			return getSSLResults;
		}
	}
}
