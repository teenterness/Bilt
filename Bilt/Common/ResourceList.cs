using System.Collections.Generic;

namespace Bilt.Common
{
	public class ResourceList<T> : Resource where T : Resource
	{
		public IEnumerable<T> Resources { get; set; }
	}
}
