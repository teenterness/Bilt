using Bilt.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bilt.BiltPoints
{
	public class TransactionResource : Resource
	{
		[ReadOnly(true)]
		public string Id { get; set; }

		[Required]
		public int? Units { get; set; }
		
		[Required]
		public bool? IsDebit { get; set; }

		[ReadOnly(true)]
		public int Points { get; set; }

		[ReadOnly(true)]
		public int? LoyaltyProgramId { get; set; }
	}
}
