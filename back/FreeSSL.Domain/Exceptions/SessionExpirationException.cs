using System;

namespace FreeSSL.Domain.Exceptions
{
	public class SessionExpirationException : Exception, IWithHumanOutput
	{
		//Your Session was be expired, please recreate it again
		public string HumanMsgKey => "session_expired";
	}
}
