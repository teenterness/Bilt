namespace Bilt.Transaction.Domain.Command
{
	public class DebitTransactionCommand
	{
		public string TransactionId { get; set; }

		public int DebitPoints { get; set; }
	}
}
