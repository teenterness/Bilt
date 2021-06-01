namespace Bilt.Transaction.Domain.Event
{
	public class TransactionDebitedEvent : Common.Event
	{
		public string TransactionId { get; set; }

		public int PointsDebited { get; set; }
	}
}
