using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSSL.Domain
{
	public class SessionExpirationException : Exception, IWithHumanOutput
	{
		public string HumanMsg => "Your Session was be expired, please recreate it again";
	}
}
