using Bilt.Common;
using Bilt.Transaction.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bilt
{
	public class BiltDatabase : IBiltDatabase
	{
		private readonly List<Event> transactionAggragateEvents = new List<Event>
		{
			new TransactionDebitedEvent
			{
				PointsDebited = 100,
				TransactionId = Guid.NewGuid().ToString(),
				Version = 1
			},
			new TransactionCreditedEvent
			{
				PointsCredited = 50,
				TransactionId = Guid.NewGuid().ToString(),
				LoyaltyProgramId = 1,
				Version = 1
			},
		};
		private readonly object lockObj = new object();

		public IEnumerable<Event> GetTransactionEvents()
		{
			return transactionAggragateEvents.OrderBy(e => e.Version);
		}

		public void Add(Event evnt, Aggregate aggregate)
		{
			if (this.GetAggregateVersionPersisted() == aggregate.GetVersion())
			{
				lock(lockObj)
				{
					var aggregateVersion = this.GetAggregateVersionPersisted();
					if (aggregateVersion == aggregate.GetVersion())
					{
						evnt.Version += 1;
						transactionAggragateEvents.Add(evnt);
						return;
					}
				}
			}
			throw new Exception("Version mismatch");
		}

		private int GetAggregateVersionPersisted()
		{
			return transactionAggragateEvents.Max(e => e.Version);
		}
	}
}
