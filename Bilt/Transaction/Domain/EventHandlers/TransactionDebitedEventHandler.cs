using Bilt.Common;
using System.Threading.Tasks;

namespace Bilt.Transaction.Domain.EventHandlers
{
	public class TransactionDebitedEventHandler : IEventHandler
	{
		private readonly IBiltDatabase biltDatabase;

		public TransactionDebitedEventHandler(IBiltDatabase biltDatabase)
		{
			this.biltDatabase = biltDatabase;
		}

		public Task HandleEvent(Common.Event evnt, Aggregate aggregate)
		{
			biltDatabase.Add(evnt, aggregate);
			return Task.CompletedTask;
		}
	}
}
