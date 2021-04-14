using ESauce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESauce.Core
{
    public class InMemorySnapshotRepository<TAggregate> : ISnapshotRepository<TAggregate> where TAggregate : AggregateBase
    {
        private readonly List<TAggregate> snapshots = new();

        public Task<TAggregate?> GetSnapshot(Guid id)
        {
            return Task.FromResult(snapshots.SingleOrDefault(s => s.AggregateId == id));
        }

        public Task PersistSnapshot(TAggregate aggregate)
        {
            var existingIndex = snapshots.FindIndex(s => s.AggregateId == aggregate.AggregateId);
            if (existingIndex > -1)
            {
                snapshots[existingIndex] = aggregate;
            }
            else
            {
                snapshots.Add(aggregate);
            }

            return Task.CompletedTask;
        }
    }
}
