using System.Collections.Generic;
using Bilt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bilt.Controllers
{
	[ApiController]
	public class ApiController : ControllerBase
	{
		public const string ROUTE = "/api";

		[HttpGet(ROUTE)]
		public IActionResult Get()
		{
			var routeResource = new RouteResource
			{
				Links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Method = "get",
						Href = ROUTE
					},
					new Link
					{
						Rel = "loyalty_programs",
						Method = "get",
						Href = LoyaltyProgramController.ROUTE
					},
					new Link
					{
						Rel = "transaction",
						Method = "get",
						Href = TransactionController.ROUTE
					}
				}
			};
			return Ok(routeResource);
		}
	}
}
