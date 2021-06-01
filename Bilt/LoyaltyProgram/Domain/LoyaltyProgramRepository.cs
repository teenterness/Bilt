using System.Collections.Generic;
using System.Linq;

namespace Bilt.LoyaltyProgram.Domain
{
	public class LoyaltyProgramRepository : ILoyaltyProgramRepository
	{
		private List<LoyaltyProgram> loyaltyPrograms = new List<LoyaltyProgram>
		{
			new LoyaltyProgram
			{
				Id = 1,
				Name = "Airline",
				BiltPointsPerProgramPoint = 2,
			},
			new LoyaltyProgram
			{
				Id = 2,
				Name = "Online Store",
				BiltPointsPerProgramPoint = 3,
			},
			new LoyaltyProgram
			{
				Id = 3,
				Name = "Grocery",
				BiltPointsPerProgramPoint = 1,
			},
		};

		public IEnumerable<LoyaltyProgram> GetAll()
		{
			return loyaltyPrograms;
		}

		public LoyaltyProgram GetSingle(int loyaltyProgramId)
		{
			return loyaltyPrograms.SingleOrDefault(lp => lp.Id == loyaltyProgramId);
		}
	}
}
