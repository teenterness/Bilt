using Bilt.Common;
using Bilt.Transaction.Domain.Command;

namespace Bilt.Transaction.Domain
{
	public interface ITransactionService
	{
		ServiceResponse HandleCommand(CreditTransactionCommand creditTransactionCommand);

		ServiceResponse HandleCommand(DebitTransactionCommand creditTransactionCommand);
	}
}
