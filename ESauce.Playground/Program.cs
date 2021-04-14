using ESauce.Core;
using ESauce.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using ESauce.Configuration;

namespace ESauce.Playground
{
    class ProductAggregate : AggregateBase
    {
        public int Quantity { get; set; } = 0;
    }

    class AddProductsEvent
    {
        public int Amount { get; set; } = 0;
    }

    class CreateProductHandler : IEventHandler<AddProductsEvent, ProductAggregate>
    {
        public Task<ProductAggregate?> Handle(ESauceEvent evt, AddProductsEvent payload, ProductAggregate? prevState)
        {
            var agg = prevState ?? new ProductAggregate();
            agg.Quantity += payload.Amount;

            return Task.FromResult(agg)!;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var dep = new ServiceCollection();
            dep.AddSingleton<IEventRepository, InMemoryEventRepository>();

            dep.AddAggregate<ProductAggregate>(cfg =>
            {
                cfg.AddEventHandler<AddProductsEvent, CreateProductHandler>();
            });

            var serviceProvider = dep.BuildServiceProvider();
            var service = serviceProvider.GetService<ESauceService<ProductAggregate>>()!;
            var aggId = Guid.NewGuid();

            for (var i = 0; i < 10000; i++)
            {
                await service!.AddEvent(aggId, new AddProductsEvent
                {
                    Amount = 5
                });
            }

            var p = await service.GetAggregate(aggId);

            Debug.Assert(p.Quantity == 50000);
            
        }
    }
}
