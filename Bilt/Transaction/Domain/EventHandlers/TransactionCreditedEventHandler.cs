using Bilt.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bilt.Transaction.Domain.EventHandlers
{
	public class TransactionCreditedEventHandler : IEventHandler
	{
		private readonly IHttpClientWrapper httpClient;
		private readonly IBiltDatabase biltDatabase;

		public TransactionCreditedEventHandler(IHttpClientWrapper httpClient, IBiltDatabase biltDatabase)
		{
			this.httpClient = httpClient;
			this.biltDatabase = biltDatabase;
		}

		public async Task HandleEvent(Common.Event evnt, Aggregate aggregate)
		{
			// Wrap in a transaction
			biltDatabase.Add(evnt, aggregate);
			await httpClient.RequestAsync(new HttpRequestMessage
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri("someloyaltyprogram/transaction")
			});
		}
	}
}
