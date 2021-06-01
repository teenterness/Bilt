using Bilt.Transaction.Domain;

namespace Bilt.BiltPoints.Domain
{
	public class BiltPointsRepository : IBiltPointsRepository
	{
		private readonly ITransactionRepository transactionRepository;

		public BiltPointsRepository(ITransactionRepository transactionRepository)
		{
			this.transactionRepository = transactionRepository;
		}

		public Points Get()
		{
			var points = 0;
			foreach (var transaction in transactionRepository.GetAll())
			{
				if (transaction.IsDebit)
				{
					points += transaction.Points;
				}
				else
				{
					points -= transaction.Points;
				}
			}

			return new Points
			{
				Total = points
			};
		}
	}
}
