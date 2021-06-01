using System.Collections.Generic;

namespace Bilt.Common
{
	public abstract class Resource
	{
		public IEnumerable<Link> Links { get; set; }
	}
}
