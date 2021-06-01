using Bilt.Common;
using Bilt.LoyaltyProgram;
using Bilt.LoyaltyProgram.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bilt.Controllers
{
	[ApiController]
	public class LoyaltyProgramController : ControllerBase
	{
		public const string ROUTE = ApiController.ROUTE + "/loyaltyprogram";
		public const string ROUTE_SINGLE = ROUTE + "/{loyaltyprogramid}";

		private readonly ILoyaltyProgramRepository loyaltyProgramRepository;

		public LoyaltyProgramController(ILoyaltyProgramRepository loyaltyProgramRepository)
		{
			this.loyaltyProgramRepository = loyaltyProgramRepository;
		}

		[HttpGet(ROUTE)]
		public IActionResult Get()
		{
			var loyaltyPrograms = loyaltyProgramRepository.GetAll();

			var resources = new ResourceList<LoyaltyProgramResource>
			{
				Resources = loyaltyPrograms.Select(lp => LoyaltyProgramToResource(lp)),
				Links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Method = "get",
						Href = ROUTE
					},
				}
			};

			return Ok(resources);
		}

		[HttpGet(ROUTE_SINGLE)]
		public IActionResult GetSingle(int loyaltyprogramid)
		{
			var loyaltyProgram = loyaltyProgramRepository.GetSingle(loyaltyprogramid);
			if (loyaltyProgram == null)
			{
				return NotFound();
			}

			return Ok(LoyaltyProgramToResource(loyaltyProgram));
		}

		private LoyaltyProgramResource LoyaltyProgramToResource(LoyaltyProgram.Domain.LoyaltyProgram loyaltyProgram)
		{
			return new LoyaltyProgramResource
			{
				Id = loyaltyProgram.Id,
				Name = loyaltyProgram.Name,
				BiltPointsPerProgramPoint = loyaltyProgram.BiltPointsPerProgramPoint,
				Links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Method = "get",
						Href = ROUTE_SINGLE.FormatNamed(new
						{
							loyaltyprogramid = loyaltyProgram.Id
						})
					}
				}
			};
		}
	}
}
