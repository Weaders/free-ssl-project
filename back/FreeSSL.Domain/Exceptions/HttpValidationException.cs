using Certes.Acme;
using System;
using System.Collections.Generic;
using System.Linq;
using static FreeSSL.Domain.SSLService.ISSLCtrlService;

namespace FreeSSL.Domain.Exceptions
{
	public class HttpValidationException : Exception, IWithHumanOutput
	{
		public IEnumerable<FailedDomainValidation> FailedValidations { get; }

		public string HumanMsg => string.Join("\r\n", FailedValidations.Select(v => $"Can not find by address {v.Location}, file with value {v.ExceptionValue}"));

		public HttpValidationException(IEnumerable<FailedDomainValidation> failedDomainValidations)
		{
			FailedValidations = failedDomainValidations;
		}

	}

	public record FailedDomainValidation(string Location, string ExceptionValue)
	{
		public FailedDomainValidation(HttpChallengeResult challenge) : this(challenge.Location.ToString(), challenge.Token) { }
	}

}
