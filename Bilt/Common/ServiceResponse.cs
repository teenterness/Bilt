using System;
using System.Linq;

namespace Bilt.Common
{
	public class ServiceResponse
	{
		public ServiceResponse(Aggregate aggregate)
		{
			WasSuccess = !aggregate.HasErrors();
			ErrorMessage = string.Join("\r\n", aggregate.GetErrors().Select(e => e.ErrorMessage()));
		}

		public ServiceResponse(Exception ex)
		{
			WasSuccess = false;
			ErrorMessage = ex.Message;
		}

		public bool WasSuccess { get; }

		public string ErrorMessage { get; }
	}
}
