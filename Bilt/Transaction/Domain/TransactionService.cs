using Bilt.Common;
using Bilt.Transaction.Domain.Command;
using System;
using System.Collections.Generic;

namespace Bilt.Transaction.Domain
{
	public class TransactionService : ITransactionService
	{
		public ITransactionAggregateRepository transactionAggregateRepository;
		private readonly Dictionary<Type, IEventHandler> eventHandlers;

		public TransactionService(ITransactionAggregateRepository transactionAggregateRepository, Dictionary<Type, IEventHandler> eventHandlers)
		{
			this.transactionAggregateRepository = transactionAggregateRepository;
			this.eventHandlers = eventHandlers;
		}

		public ServiceResponse HandleCommand(CreditTransactionCommand creditTransactionCommand)
		{
			try
			{
				var aggregate = transactionAggregateRepository.GetTransactionAggregate();
				aggregate.HandleCommand(creditTransactionCommand);
				if (!aggregate.HasErrors())
				{
					foreach (var evnt in aggregate.GetStagedEvents())
					{
						eventHandlers[evnt.GetType()].HandleEvent(evnt, aggregate);
					}
				}

				return new ServiceResponse(aggregate);
			}
			catch (Exception ex)
			{
				return new ServiceResponse(ex);
			}
		}

		public ServiceResponse HandleCommand(DebitTransactionCommand debitTransactionCommand)
		{
			try
			{
				var aggregate = transactionAggregateRepository.GetTransactionAggregate();
				aggregate.HandleCommand(debitTransactionCommand);
				if (!aggregate.HasErrors())
				{
					foreach (var evnt in aggregate.GetStagedEvents())
					{
						eventHandlers[evnt.GetType()].HandleEvent(evnt, aggregate);
					}
				}

				return new ServiceResponse(aggregate);
			}
			catch (Exception ex)
			{
				return new ServiceResponse(ex);
			}
		}
	}
}
