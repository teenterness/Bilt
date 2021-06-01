namespace Bilt.Transaction.Domain
{
	public interface ITransactionAggregateRepository
	{
		TransactionAggregate GetTransactionAggregate();
	}
}
