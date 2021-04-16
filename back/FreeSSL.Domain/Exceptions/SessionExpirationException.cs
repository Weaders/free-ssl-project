using System;

namespace FreeSSL.Domain.Exceptions
{
	public class SessionExpirationException : Exception, IWithHumanOutput
	{
		public string HumanMsg => "Your Session was be expired, please recreate it again";
	}
}
