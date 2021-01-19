﻿using FreeSSL.Domain;
using FreeSSL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FreeSSL.Controllers
{
	[Route("api/ssl")]
	[ApiController]
	public class SSLController : Controller
	{

		private readonly ISSLCtrlService _sslCtrl;
		private readonly AccountDataOptions _accountDataOptions;

		public SSLController(ISSLCtrlService sslCtrl, IOptions<AccountDataOptions> accountDataOptions)
		{
			_sslCtrl = sslCtrl;
			_accountDataOptions = accountDataOptions.Value;
		}

		[HttpPost("start")]
		public async Task<IActionResult> Start([FromBody] StartMsg msg)
			=> Json(await _sslCtrl.StartGetSSLAsync(msg.Domains, new ISSLCtrlService.AccountData
			{ 
				AccountPemKey = _accountDataOptions.AccountPemKey
			}));

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
