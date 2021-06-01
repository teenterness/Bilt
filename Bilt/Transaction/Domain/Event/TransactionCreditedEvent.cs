namespace Bilt.Transaction.Domain.Event
{
	public class TransactionCreditedEvent : Common.Event
	{
		public string TransactionId { get; set; }

		public int PointsCredited { get; set; }

		public int LoyaltyProgramId { get; set; }
	}
}
