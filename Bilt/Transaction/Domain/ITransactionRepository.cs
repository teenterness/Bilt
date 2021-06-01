using System.Collections.Generic;

namespace Bilt.Transaction.Domain
{
	public interface ITransactionRepository
	{
		IEnumerable<Transaction> GetAll();
	}
}
