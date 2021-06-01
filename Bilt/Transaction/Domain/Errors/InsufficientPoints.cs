using Bilt.Common;

namespace Bilt.Transaction.Domain.Errors
{
	public class InsufficientPoints : DomainError
	{
		public override string ErrorMessage()
		{
			return "Insufficient points to spend";
		}
	}
}
