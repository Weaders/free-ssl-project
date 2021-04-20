using FreeSSL.Domain.SSLService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FreeSSL.Controllers
{
	[Route("v1/ssl")]
	[ApiController]
	public class SSLController : Controller
	{

		private readonly ISSLCtrlService _sslCtrl;

		public SSLController(ISSLCtrlService sslCtrl)
		{
			_sslCtrl = sslCtrl;
		}

		[HttpPost("start")]
		public async Task<IActionResult> Start([FromBody] StartMsg msg)
			=> Json(await _sslCtrl.StartGetSSLAsync(msg.Domains));

		[HttpPost("download")]
		public async Task<IActionResult> DownloadCertificate([FromBody] DownloadMsg msg)
		{
			if (Guid.TryParse(msg.Id, out var sessionId))
			{
				return Ok(await _sslCtrl.TryDownloadCert(sessionId));
			}

			return NotFound();

		}

		public class StartMsg
		{
			public string[] Domains { get; set; }
		}

		public class DownloadMsg
		{
			public string Id { get; set; }
		}
	}
}
