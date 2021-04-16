using Certes;
using static FreeSSL.Domain.SSLService.ISSLCtrlService;

namespace FreeSSL.Models
{
	public class AccountDataOptions
	{
		public const string KEY_SETTING = "AccountData";

		public string AccountPemKey { get; set; }

		public string Email { get; set; }

		public SSLAccountData ToSSLAccData()
				=> new SSLAccountData()
				{ 
					AccountPemKey = AccountPemKey,
					Email = Email
				};

	}

	public class CsrInfoOptions
	{ 
		public string CountryName { get; set; }
		public string State { get; set; }
		public string Locality { get; set; }
		public string Organization { get; set; }
		public string OrganizationUnit { get; set; }
		public string CommonName { get; set; }

		public CsrInfo ToCsrfInfo()
			=> new CsrInfo()
			{
			};
	}
}
