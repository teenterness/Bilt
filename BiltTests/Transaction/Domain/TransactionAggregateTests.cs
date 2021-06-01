using Bilt.Common;
using Bilt.Transaction.Domain;
using Bilt.Transaction.Domain.Command;
using Bilt.Transaction.Domain.Errors;
using Bilt.Transaction.Domain.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BiltTests.Transaction.Domain
{
	[TestClass]
	public class TransactionAggregateTests
	{
		private TransactionAggregate subject;

		[TestMethod]
		public void HandleCommand_Debit_HasNegativeAmount()
		{
			var command = new DebitTransactionCommand
			{
				DebitPoints = -1
			};

			subject = new TransactionAggregate(new List<Event>());
			subject.HandleCommand(command);

			Assert.IsTrue(subject.HasErrors());
			Assert.AreEqual(1, subject.GetErrors().Count());
			Assert.IsInstanceOfType(subject.GetErrors().Single(), typeof(PointsCantBeNegative));
		}

		[TestMethod]
		public void HandleCommand_Debit_Success()
		{
			var command = new DebitTransactionCommand
			{
				TransactionId = "123",
				DebitPoints = 10
			};

			subject = new TransactionAggregate(new List<Event>());
			subject.HandleCommand(command);

			Assert.IsFalse(subject.HasErrors());
			Assert.AreEqual(1, subject.GetStagedEvents().Count());

			var evnt = (TransactionDebitedEvent)subject.GetStagedEvents().First();
			Assert.AreEqual(command.DebitPoints, evnt.PointsDebited);
			Assert.AreEqual(command.TransactionId, evnt.TransactionId);
		}

		[TestMethod]
		public void HandleCommand_Credit_HasNegativeAmount()
		{
			var command = new CreditTransactionCommand
			{
				CreditPoints = -1
			};

			subject = new TransactionAggregate(new List<Event>());
			subject.HandleCommand(command);

			Assert.IsTrue(subject.HasErrors());
			Assert.AreEqual(1, subject.GetErrors().Count());
			Assert.IsInstanceOfType(subject.GetErrors().Single(), typeof(PointsCantBeNegative));
		}

		[TestMethod]
		public void HandleCommand_Credit_InsufficientPoints()
		{
			var command = new CreditTransactionCommand
			{
				CreditPoints = 50
			};

			subject = new TransactionAggregate(new List<Event>
			{
				new TransactionDebitedEvent
				{
					PointsDebited = 25,
					Version = 1
				}
			});

			subject.HandleCommand(command);

			Assert.IsTrue(subject.HasErrors());
			Assert.AreEqual(1, subject.GetErrors().Count());
			Assert.IsInstanceOfType(subject.GetErrors().Single(), typeof(InsufficientPoints));
		}

		[TestMethod]
		public void HandleCommand_Credit_Success()
		{
			var command = new CreditTransactionCommand
			{
				TransactionId = "345",
				CreditPoints = 25,
				LoyaltyProgramId = 2
			};

			subject = new TransactionAggregate(new List<Event>
			{
				new TransactionDebitedEvent
				{
					PointsDebited = 25,
					Version = 1
				}
			});

			subject.HandleCommand(command);

			Assert.IsFalse(subject.HasErrors());
			Assert.AreEqual(1, subject.GetStagedEvents().Count());

			var evnt = (TransactionCreditedEvent)subject.GetStagedEvents().First();
			Assert.AreEqual(command.CreditPoints, evnt.PointsCredited);
			Assert.AreEqual(command.LoyaltyProgramId, evnt.LoyaltyProgramId);
			Assert.AreEqual(command.TransactionId, evnt.TransactionId);
		}
	}
}
