using System;
using ESauce.Core;
using ESauce.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ESauce.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventHandler<TPayload, TAggregate, TEventHandler>(this IServiceCollection self)
            where TAggregate : AggregateBase
            where TEventHandler : class, IEventHandler<TPayload, TAggregate>
        {
            return self.AddTransient<IEventHandler<TPayload, TAggregate>, TEventHandler>();
        }

        public static IServiceCollection AddAggregate<TAggregate>(this IServiceCollection self, Action<AggregateConfiguration<TAggregate>> cfg) 
            where TAggregate : AggregateBase
        {
            var handlerMapping = new HandlerMappingProvider<TAggregate>();
            var reg = new AggregateConfiguration<TAggregate>(self, handlerMapping);
            cfg(reg);
            
            self.AddSingleton<IHandlerMappingProvider<TAggregate>>(handlerMapping);
            self.AddTransient<ESauceService<TAggregate>>();
            
            return self;
        }
    }
}
