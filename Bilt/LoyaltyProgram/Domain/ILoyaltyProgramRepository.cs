using System.Collections.Generic;

namespace Bilt.LoyaltyProgram.Domain
{
	public interface ILoyaltyProgramRepository
	{
		IEnumerable<LoyaltyProgram> GetAll();

		LoyaltyProgram GetSingle(int loyaltyProgramId);
	}
}
