using System;
using System.Collections.Generic;
using System.Linq;
using Bilt;
using Bilt.Common;
using Bilt.LoyaltyProgram.Domain;
using Bilt.Transaction.Domain;
using Bilt.Transaction.Domain.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BiltTests.Transaction.Domain
{
	[TestClass]
	public class TransactionRepositoryTest
	{
		private IBiltDatabase biltDatabase;
		private LoyaltyProgramRepositoryStub loyaltyProgramRepositoryStub;

		private ITransactionRepository subject;

		[TestInitialize]
		public void Setup()
		{
			biltDatabase = new BiltDatabaseMock();
			loyaltyProgramRepositoryStub = new LoyaltyProgramRepositoryStub();
			subject = new TransactionRepository(biltDatabase, loyaltyProgramRepositoryStub);
		}

		[TestMethod]
		public void GetAll_WorksWithDebit()
		{
			var transactionDebitedEvent = new TransactionDebitedEvent
			{
				TransactionId = Guid.NewGuid().ToString(),
				PointsDebited = 100,
				Version = 1
			};
			biltDatabase.Add(transactionDebitedEvent, null);

			var transactions = subject.GetAll().ToList();

			Assert.AreEqual(1, transactions.Count);
			var debitTransaction = transactions[0];
			Assert.AreEqual(transactionDebitedEvent.TransactionId, debitTransaction.Id);
			Assert.IsTrue(debitTransaction.IsDebit);
			Assert.IsNull(debitTransaction.LoyaltyProgramId);
			Assert.AreEqual(transactionDebitedEvent.PointsDebited, debitTransaction.Points);
			Assert.AreEqual(transactionDebitedEvent.PointsDebited, debitTransaction.Units);
		}

		[TestMethod]
		public void GetAll_WorksWithCredit()
		{
			var transactionCreditedEvent = new TransactionCreditedEvent
			{
				TransactionId = Guid.NewGuid().ToString(),
				PointsCredited = 100,
				LoyaltyProgramId = 3,
				Version = 1
			};
			loyaltyProgramRepositoryStub.SetSingle(new LoyaltyProgram
			{
				BiltPointsPerProgramPoint = 2,
				Id = 3
			});
			biltDatabase.Add(transactionCreditedEvent, null);

			var transactions = subject.GetAll().ToList();

			Assert.AreEqual(1, transactions.Count);
			var creditTransaction = transactions[0];
			Assert.AreEqual(transactionCreditedEvent.TransactionId, creditTransaction.Id);
			Assert.IsFalse(creditTransaction.IsDebit);
			Assert.AreEqual(transactionCreditedEvent.LoyaltyProgramId, creditTransaction.LoyaltyProgramId);
			Assert.AreEqual(transactionCreditedEvent.PointsCredited, creditTransaction.Points);
			Assert.AreEqual(50, creditTransaction.Units);

		}

		private class BiltDatabaseMock : IBiltDatabase
		{
			private List<Event> transactionEvents = new List<Event>();
			
			public void Add(Event evnt, Aggregate aggregate)
			{
				transactionEvents.Add(evnt);
			}

			public IEnumerable<Event> GetTransactionEvents()
			{
				return transactionEvents;
			}
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
	}
}
