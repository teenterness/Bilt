namespace Bilt.Transaction.Domain
{
	public class Transaction
	{
		public string Id { get; set; }

		public int Units { get; set; }

		public bool IsDebit { get; set; }

		public int Points { get; set; }

		public int? LoyaltyProgramId { get; set; }
	}
}
