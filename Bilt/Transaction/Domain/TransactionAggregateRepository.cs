namespace Bilt.Transaction.Domain
{
	public class TransactionAggregateRepository : ITransactionAggregateRepository
	{
		private readonly IBiltDatabase biltDatabase;

		public TransactionAggregateRepository(IBiltDatabase biltDatabase)
		{
			this.biltDatabase = biltDatabase;
		}

		public TransactionAggregate GetTransactionAggregate()
		{
			return new TransactionAggregate(biltDatabase.GetTransactionEvents());
		}
	}
}
