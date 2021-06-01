using System.Threading.Tasks;

namespace Bilt.Common
{
	public interface IEventHandler
	{
		Task HandleEvent(Event evnt, Aggregate aggregate);
	}
}
