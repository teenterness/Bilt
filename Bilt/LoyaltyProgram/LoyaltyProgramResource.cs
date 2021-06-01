using Bilt.Common;

namespace Bilt.LoyaltyProgram
{
	public class LoyaltyProgramResource : Resource
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int BiltPointsPerProgramPoint { get; set; }
	}
}
