using ESauce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESauce.Core
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly List<ESauceEvent> events = new();

        public Task AddEvent(ESauceEvent evt)
        {
            var ver = GetNewestAggregateVersion(evt.AggregateId) ?? 0;
            evt.Version = ver + 1;

            events.Add(evt);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<ESauceEvent>> StreamEvents(Guid aggregateId, int? lastVersion)
        {
            return Task.FromResult(events.Where(e => e.AggregateId == aggregateId && (!lastVersion.HasValue || e.Version > lastVersion)));
        }

        private int? GetNewestAggregateVersion(Guid aggregateId)
        {
            return events.LastOrDefault(e => e.AggregateId == aggregateId)?.Version;
        }
    }
}
