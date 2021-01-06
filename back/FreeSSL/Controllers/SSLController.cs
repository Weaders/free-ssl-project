using FreeSSL.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeSSL.Controllers
{
	[Route("api/ssl")]
	[ApiController]
	public class SSLController : ControllerBase
	{

		private readonly ISSLCtrlService _sslCtrl;

		public SSLController(ISSLCtrlService sslCtrl)
		{
			_sslCtrl = sslCtrl;
		}

		[HttpPost("start")]
		public async Task<IActionResult> Start([FromBody]StartMsg msg)
		{
			var result = await _sslCtrl.StartGetSSLAsync(msg.Domains);
			return new JsonResult(result);
		}

		public class StartMsg
		{
			public string[] Domains { get; set; }

		}
	}
}
