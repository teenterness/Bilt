using Bilt.BiltPoints;
using Bilt.Common;
using Bilt.Controllers;
using Bilt.LoyaltyProgram.Domain;
using Bilt.Transaction.Domain;
using Bilt.Transaction.Domain.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;

namespace BiltTests.Transaction
{
	[TestClass]
	public class TransactionControllerTests
	{
		private LoyaltyProgramRepositoryStub loyaltyProgramRepositoryStub;
		private TransactionServiceStub transactionServiceStub;

		private TransactionController subject;

		[TestInitialize]
		public void Setup()
		{
			loyaltyProgramRepositoryStub = new LoyaltyProgramRepositoryStub();
			transactionServiceStub = new TransactionServiceStub();
			subject = new TransactionController(null, loyaltyProgramRepositoryStub, transactionServiceStub);
			subject.ControllerContext.HttpContext = new DefaultHttpContext();
		}

		[TestMethod]
		public void Post_DebitTransaction_Success()
		{
			var transactionResource = new TransactionResource
			{
				IsDebit = true,
				Units = 50
			};

			var response = (CreatedResult)subject.Post(transactionResource);

			var actualDebitTransactionCommand = transactionServiceStub.DebitTransactionCommand;
			Assert.AreEqual(transactionResource.Units.Value, actualDebitTransactionCommand.DebitPoints);

			Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
			Assert.AreEqual($"/api/transaction/{actualDebitTransactionCommand.TransactionId}", response.Location);
		}

		[TestMethod]
		public void Post_CreditTransaction_NoLoyaltyProgram()
		{
			var transactionResource = new TransactionResource
			{
				IsDebit = false,
				LoyaltyProgramId = 5,
				Units = 50
			};

			var response = (BadRequestObjectResult)subject.Post(transactionResource);

			Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
			Assert.AreEqual("Loyalty program not found", response.Value);
		}

		[TestMethod]
		public void Post_CreditTransaction_Success()
		{
			var transactionResource = new TransactionResource
			{
				IsDebit = false,
				LoyaltyProgramId = 5,
				Units = 10
			};
			loyaltyProgramRepositoryStub.SetSingle(new LoyaltyProgram
			{
				Id = 5,
				BiltPointsPerProgramPoint = 5
			});

			var response = (CreatedResult)subject.Post(transactionResource);

			var creditTransactionCommand = transactionServiceStub.CreditTransactionCommand;
			Assert.AreEqual(50, creditTransactionCommand.CreditPoints);
			Assert.AreEqual(transactionResource.LoyaltyProgramId, creditTransactionCommand.LoyaltyProgramId);

			Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
			Assert.AreEqual($"/api/transaction/{creditTransactionCommand.TransactionId}", response.Location);
		}

		private class LoyaltyProgramRepositoryStub : ILoyaltyProgramRepository
		{
			private LoyaltyProgram loyaltyProgram;

			public void SetSingle(LoyaltyProgram loyaltyProgram)
			{
				this.loyaltyProgram = loyaltyProgram;
			}

			public IEnumerable<LoyaltyProgram> GetAll()
			{
				throw new System.NotImplementedException();
			}

			public LoyaltyProgram GetSingle(int loyaltyProgramId)
			{
				return loyaltyProgram;
			}
		}

		private class TransactionServiceStub : ITransactionService
		{
			public CreditTransactionCommand CreditTransactionCommand { get; set; }
			public DebitTransactionCommand DebitTransactionCommand { get; set; }

			public ServiceResponse HandleCommand(CreditTransactionCommand creditTransactionCommand)
			{
				CreditTransactionCommand = creditTransactionCommand;
				return new ServiceResponse(new StubAggregate());
			}

			public ServiceResponse HandleCommand(DebitTransactionCommand debitTransactionCommand)
			{
				DebitTransactionCommand = debitTransactionCommand;
				return new ServiceResponse(new StubAggregate());
			}
		}

		private class StubAggregate : Aggregate
		{
			public StubAggregate() : base(new List<Event>())
			{
			}

			protected override void ReplayEvent(Event evnt)
			{
			}
		}
	}
}
