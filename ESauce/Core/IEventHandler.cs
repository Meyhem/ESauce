using ESauce.Model;
using System.Threading.Tasks;

namespace ESauce.Core
{
    public interface IEventHandler<TPayload, TAggregate> where TAggregate : AggregateBase
    {
        Task<TAggregate?> Handle(ESauceEvent evt, TPayload payload, TAggregate? prevState);
    }
}
