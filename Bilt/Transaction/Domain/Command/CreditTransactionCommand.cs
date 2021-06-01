namespace Bilt.Transaction.Domain.Command
{
	public class CreditTransactionCommand
	{
		public string TransactionId { get; set; }

		public int CreditPoints { get; set; }

		public int LoyaltyProgramId { get; set; }
	}
}
