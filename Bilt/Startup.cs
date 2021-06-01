using Bilt.BiltPoints.Domain;
using Bilt.Common;
using Bilt.LoyaltyProgram.Domain;
using Bilt.Transaction.Domain;
using Bilt.Transaction.Domain.Event;
using Bilt.Transaction.Domain.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Bilt
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			var httpClientWrapper = new HttpClientWrapper();
			var biltDatabase = new BiltDatabase();
			var loyaltyProgramRepository = new LoyaltyProgramRepository();
			var transactionRepository = new TransactionRepository(biltDatabase, loyaltyProgramRepository);
			var biltPointsRepository = new BiltPointsRepository(transactionRepository);
			var transactionAggregateRepository = new TransactionAggregateRepository(biltDatabase);
			var eventHandlers = GetEventHandlers(httpClientWrapper, biltDatabase);
			var transactionService = new TransactionService(transactionAggregateRepository, eventHandlers);

			services.AddSingleton<ILoyaltyProgramRepository>(loyaltyProgramRepository);
			services.AddSingleton<ITransactionRepository>(transactionRepository);
			services.AddSingleton<IBiltPointsRepository>(biltPointsRepository);
			services.AddSingleton<IBiltDatabase>(biltDatabase);
			services.AddSingleton<IHttpClientWrapper>(httpClientWrapper);
			services.AddSingleton<ITransactionAggregateRepository>(transactionAggregateRepository);
			services.AddSingleton<ITransactionService>(transactionService);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private Dictionary<Type, IEventHandler> GetEventHandlers(IHttpClientWrapper httpClientWrapper, IBiltDatabase biltDatabase)
		{
			return new Dictionary<Type, IEventHandler>
			{
				{ typeof(TransactionCreditedEvent), new TransactionCreditedEventHandler(httpClientWrapper, biltDatabase) },
				{ typeof(TransactionDebitedEvent), new TransactionDebitedEventHandler(biltDatabase) },
			};
		}
	}
}
