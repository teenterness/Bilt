using Bilt.LoyaltyProgram.Domain;
using Bilt.Transaction.Domain.Event;
using System.Collections.Generic;

namespace Bilt.Transaction.Domain
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly IBiltDatabase biltDatabase;
		private readonly ILoyaltyProgramRepository loyaltyProgramRepository;

		public TransactionRepository(IBiltDatabase biltDatabase, ILoyaltyProgramRepository loyaltyProgramRepository)
		{
			this.biltDatabase = biltDatabase;
			this.loyaltyProgramRepository = loyaltyProgramRepository;
		}

		public IEnumerable<Transaction> GetAll()
		{
			var transactionEvents = biltDatabase.GetTransactionEvents();
			foreach (var evnt in transactionEvents)
			{
				switch (evnt)
				{
					case TransactionCreditedEvent transactionCreditedEvent:
						yield return ConvertCredit(transactionCreditedEvent);
						break;
					case TransactionDebitedEvent transactionDebitedEvent:
						yield return ConvertDebit(transactionDebitedEvent);
						break;
				}
			}
		}

		private Transaction ConvertCredit(TransactionCreditedEvent transactionCreditedEvent)
		{
			var loyaltyProgram = loyaltyProgramRepository.GetSingle(transactionCreditedEvent.LoyaltyProgramId);

			return new Transaction
			{
				Id = transactionCreditedEvent.TransactionId,
				IsDebit = false,
				Points = transactionCreditedEvent.PointsCredited,
				Units = transactionCreditedEvent.PointsCredited / loyaltyProgram.BiltPointsPerProgramPoint,
				LoyaltyProgramId = transactionCreditedEvent.LoyaltyProgramId
			};
		}

		private Transaction ConvertDebit(TransactionDebitedEvent transactionDebitedEvent)
		{
			return new Transaction
			{
				Id = transactionDebitedEvent.TransactionId,
				IsDebit = true,
				Points = transactionDebitedEvent.PointsDebited,
				Units = transactionDebitedEvent.PointsDebited
			};
		}
	}
}
