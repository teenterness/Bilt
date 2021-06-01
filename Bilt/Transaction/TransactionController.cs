using Bilt.BiltPoints;
using Bilt.Common;
using Bilt.LoyaltyProgram.Domain;
using Bilt.Transaction.Domain;
using Bilt.Transaction.Domain.Command;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bilt.Controllers
{
	[ApiController]
	public class TransactionController : ControllerBase
	{
		public const string ROUTE = ApiController.ROUTE + "/transaction";
		public const string ROUTE_SINGLE = ROUTE + "/{transactionid}";

		private readonly ITransactionRepository transactionRepository;
		private readonly ILoyaltyProgramRepository loyaltyProgramRepository;
		private readonly ITransactionService transactionService;

		public TransactionController(
			ITransactionRepository transactionRepository,
			ILoyaltyProgramRepository loyaltyProgramRepository,
			ITransactionService transactionService)
		{
			this.transactionRepository = transactionRepository;
			this.loyaltyProgramRepository = loyaltyProgramRepository;
			this.transactionService = transactionService;
		}

		[HttpGet(ROUTE)]
		public IActionResult Get()
		{
			var transactions = transactionRepository.GetAll();
			var resourceList = new ResourceList<TransactionResource>
			{
				Resources = transactions.Select(t => TransactionToResource(t)),
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
						Rel = "post_self",
						Method = "post",
						Href = ROUTE
					}
				}
			};

			return Ok(resourceList);
		}

		[HttpGet(ROUTE_SINGLE)]
		public IActionResult Get(string transactionId)
		{
			var transaction = transactionRepository.GetAll().SingleOrDefault(t => t.Id == transactionId);
			if (transaction == null)
			{
				return NotFound();
			}

			return Ok(TransactionToResource(transaction));
		}

		[HttpPost(ROUTE)]
		public IActionResult Post([FromBody]TransactionResource transactionResource)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var transactionId = Guid.NewGuid().ToString();
			ServiceResponse serviceResponse;
			if (transactionResource.IsDebit.Value)
			{
				serviceResponse = transactionService.HandleCommand(new DebitTransactionCommand
				{
					TransactionId = transactionId,
					DebitPoints = transactionResource.Units.Value
				});
			}
			else
			{
				var loyaltyProgram = loyaltyProgramRepository.GetSingle(transactionResource.LoyaltyProgramId.Value);
				if (loyaltyProgram == null)
				{
					return BadRequest("Loyalty program not found");
				}
				serviceResponse = transactionService.HandleCommand(new CreditTransactionCommand
				{
					TransactionId = transactionId,
					CreditPoints = transactionResource.Units.Value * loyaltyProgram.BiltPointsPerProgramPoint,
					LoyaltyProgramId = loyaltyProgram.Id
				});
			}

			if (serviceResponse.WasSuccess)
			{
				return Created(ROUTE_SINGLE.FormatNamed(new
				{
					transactionid = transactionId
				}), null);
			}
			else
			{
				return BadRequest(serviceResponse.ErrorMessage);
			}
		}

		private TransactionResource TransactionToResource(Transaction.Domain.Transaction transaction)
		{
			return new TransactionResource
			{
				Id = transaction.Id,
				IsDebit = transaction.IsDebit,
				LoyaltyProgramId = transaction.LoyaltyProgramId,
				Points = transaction.Points,
				Units = transaction.Units,
				Links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Method = "get",
						Href = ROUTE_SINGLE.FormatNamed(new
						{
							transactionid = transaction.Id
						})
					}
				}
			};
		}
	}
}
