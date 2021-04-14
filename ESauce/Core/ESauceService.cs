using ESauce.Model;
using System;
using System.Threading.Tasks;

namespace ESauce.Core
{
    public class ESauceService<TAggregate>
        where TAggregate: AggregateBase
    {
        private readonly IEventRepository eventRepository;
        private readonly IHandlerMappingProvider<TAggregate> handlerMappingProvider;
        // private readonly ISnapshotRepository<TAggregate> snapshotRepository;
        private readonly IServiceProvider serviceProvider;

        public ESauceService(IEventRepository eventRepository,
            // ISnapshotRepository<TAggregate> snapshotRepository,
            IHandlerMappingProvider<TAggregate> handlerMappingProvider,
            IServiceProvider serviceProvider)
        {
            this.eventRepository = eventRepository;
            this.handlerMappingProvider = handlerMappingProvider;
            // this.snapshotRepository = snapshotRepository;
            this.serviceProvider = serviceProvider;
        }

        public async Task AddEvent<T>(Guid aggregateId, T payload)
        {
            var evt = ESauceEvent.Create(aggregateId, payload);
            await eventRepository.AddEvent(evt);
        }

        public async Task<TAggregate?> GetAggregate(Guid aggregateId)
        {
            var eventStream = await eventRepository.StreamEvents(aggregateId, null);

            TAggregate? aggregate = null;
            foreach (var evt in eventStream)
            {
                aggregate = await Apply(evt, aggregate);

                if (aggregate is not null)
                {
                    aggregate.Version = evt.Version;
                }
            }

            return aggregate;
        }

        private async Task<TAggregate?> Apply(ESauceEvent evt, TAggregate? prevState)
        {
            var handlerType = handlerMappingProvider.FindHandlerForEvent(evt.PayloadType);
            var handler = serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new Exception($"No IEventProcessor found to process event of type {evt.PayloadType}");
            }

            const string methodName = nameof(IEventHandler<object, AggregateBase>.Handle);
            var processMethod = handler.GetType().GetMethod(methodName);
            var methodParameters = new[] { evt, evt.Payload!, prevState! };

            return (processMethod?.Invoke(handler, methodParameters)) switch
            {
                Task<TAggregate?> ret => await ret,
                _ => throw new Exception($"Dynamic invocation of method {methodName} for type {evt.PayloadType} failed")
            };
        }
    }
}
