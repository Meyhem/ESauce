using System;

namespace ESauce.Model
{
    public class ESauceEvent
    {
        public int Version { get; set; } = 0;

        public Guid AggregateId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string PayloadType { get; set; } = typeof(object).AssemblyQualifiedName!;

        public object Payload { get; set; } = new();

        public static ESauceEvent Create<T>(Guid aggregateId, T payload)
        {
            _ = payload ?? throw new ArgumentNullException(nameof(payload));

            return new ESauceEvent()
            {
                AggregateId = aggregateId,
                CreatedAt = DateTime.UtcNow,
                PayloadType = payload!.GetType().Name,
                Payload = payload
            };
        }
    }
}
