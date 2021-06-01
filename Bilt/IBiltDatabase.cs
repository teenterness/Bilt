using Bilt.Common;
using System.Collections.Generic;

namespace Bilt
{
	public interface IBiltDatabase
	{
		IEnumerable<Event> GetTransactionEvents();

		void Add(Event evnt, Aggregate aggregate);
	}
}
