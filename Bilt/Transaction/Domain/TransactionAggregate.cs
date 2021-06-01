using Bilt.Transaction.Domain.Command;
using Bilt.Transaction.Domain.Errors;
using Bilt.Transaction.Domain.Event;
using System.Collections.Generic;

namespace Bilt.Transaction.Domain
{
	public class TransactionAggregate : Common.Aggregate
	{
		private int points = 0;

		public TransactionAggregate(IEnumerable<Common.Event> events) 
			: base(events)
		{
		}

		public void HandleCommand(DebitTransactionCommand debitTransactionCommand)
		{
			if (debitTransactionCommand.DebitPoints < 0)
			{
				RecordError(new PointsCantBeNegative());
			}

			StageEvent(new TransactionDebitedEvent
			{
				TransactionId = debitTransactionCommand.TransactionId,
				PointsDebited = debitTransactionCommand.DebitPoints,
			});
		}

		public void HandleCommand(CreditTransactionCommand creditTransactionCommand)
		{
			if (creditTransactionCommand.CreditPoints < 0)
			{
				RecordError(new PointsCantBeNegative());
				return;
			}

			if (points - creditTransactionCommand.CreditPoints < 0)
			{
				RecordError(new InsufficientPoints());
			}

			StageEvent(new TransactionCreditedEvent
			{
				TransactionId = creditTransactionCommand.TransactionId,
				PointsCredited = creditTransactionCommand.CreditPoints,
				LoyaltyProgramId = creditTransactionCommand.LoyaltyProgramId
			});
		}

		protected override void ReplayEvent(Common.Event evnt)
		{
			switch (evnt)
			{
				case TransactionCreditedEvent transactionCreddittedEvent:
					points -= transactionCreddittedEvent.PointsCredited;
					break;
				case TransactionDebitedEvent transactionDebittedEvent:
					points += transactionDebittedEvent.PointsDebited;
					break;
			}
		}
	}
}
