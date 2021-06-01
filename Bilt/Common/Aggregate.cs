using System;
using System.Collections.Generic;
using System.Linq;

namespace Bilt.Common
{
	public abstract class Aggregate
	{
		private int version = 0;

		private List<Event> stagedEvents = new List<Event>();
		private List<DomainError> errors = new List<DomainError>();

		public Aggregate(IEnumerable<Event> events)
		{
			foreach (var evnt in events.OrderBy(e => e.Version))
			{
				ReplayEvent(evnt);
				version = evnt.Version;
			}
		}

		public int GetVersion()
		{
			return version;
		}

		public IEnumerable<Event> GetStagedEvents()
		{
			return stagedEvents;
		}

		public bool HasErrors()
		{
			return errors.Any();
		}

		public IEnumerable<DomainError> GetErrors()
		{
			return errors;
		}

		protected void StageEvent(Event e)
		{
			stagedEvents.Add(e);
		}

		protected void RecordError(DomainError error)
		{
			errors.Add(error);
		}

		protected abstract void ReplayEvent(Event evnt);
	}
}
