using Certes.Acme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FreeSSL.Domain.SSLService.ISSLCtrlService;

namespace FreeSSL.Domain.SSLService
{
	public record MakeSSLContext(IOrderContext OrderCtrx, IChallengeContext[] Challenges, IEnumerable<HttpChallengeResult> HttpChallengeResults);
}
