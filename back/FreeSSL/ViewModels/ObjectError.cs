using FreeSSL.Domain;
using FreeSSL.Domain.Exceptions;
using System;

namespace FreeSSL.ViewModels
{
	public class ObjectError
	{
		public ObjectError()
		{
			Msg = "There some problem on site side, we work for fix it";
		}

		public ObjectError(IWithHumanOutput exception)
		{
			Msg = exception.HumanMsg;
		}

		public ObjectError(Exception e) : this()
		{

		}

		public string Msg { get; set; }
	}
}
