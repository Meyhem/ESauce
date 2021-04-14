using ESauce.Model;
using System;
using System.Threading.Tasks;

namespace ESauce.Core
{
    public interface ISnapshotRepository<TAggregate> where TAggregate : AggregateBase
    {
        Task<TAggregate?> GetSnapshot(Guid id);
        Task PersistSnapshot(TAggregate aggregate);
    }
}