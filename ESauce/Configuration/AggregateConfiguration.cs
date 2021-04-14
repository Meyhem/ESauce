using ESauce.Core;
using ESauce.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ESauce.Configuration
{
    public class AggregateConfiguration<TAggregate> where TAggregate: AggregateBase
    {
        private readonly IServiceCollection serviceDescriptors;
        private readonly IHandlerMappingProvider<TAggregate> handlerMapping;

        public AggregateConfiguration(IServiceCollection serviceDescriptors, 
            IHandlerMappingProvider<TAggregate> handlerMapping)
        {
            this.serviceDescriptors = serviceDescriptors;
            this.handlerMapping = handlerMapping;
        }

        public AggregateConfiguration<TAggregate> AddEventHandler<TPayload, TEventHandler>()
            where TEventHandler: class, IEventHandler<TPayload, TAggregate>
        {
            serviceDescriptors.AddEventHandler<TPayload, TAggregate, TEventHandler>();
            handlerMapping.AddHandlerMapping<TPayload, IEventHandler<TPayload, TAggregate>>();
            
            return this;
        }
    }
}