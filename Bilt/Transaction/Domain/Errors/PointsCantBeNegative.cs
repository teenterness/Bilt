using Bilt.Common;

namespace Bilt.Transaction.Domain.Errors
{
	public class PointsCantBeNegative : DomainError
	{
		public override string ErrorMessage()
		{
			return "Points can't be negative";
		}
	}
}
