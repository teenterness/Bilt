using Bilt.BiltPoints;
using Bilt.BiltPoints.Domain;
using Bilt.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bilt.Controllers
{
	[ApiController]
	public class BiltPointsController : ControllerBase
	{
		public const string ROUTE = ApiController.ROUTE + "/points";

		private readonly IBiltPointsRepository biltPointsRepository;

		public BiltPointsController(IBiltPointsRepository biltPointsRepository)
		{
			this.biltPointsRepository = biltPointsRepository;
		}

		[HttpGet(ROUTE)]
		public IActionResult Get()
		{
			var points = biltPointsRepository.Get();

			return Ok(PointsToResource(points));
		}

		private BiltPointsResource PointsToResource(Points points)
		{
			return new BiltPointsResource
			{
				Points = points.Total,
				Links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Method = "get",
						Href = ROUTE
					}
				}
			};
		}
	}
}
