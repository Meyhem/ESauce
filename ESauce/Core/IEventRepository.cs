using ESauce.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESauce.Core
{
    public interface IEventRepository
    {
        Task AddEvent(ESauceEvent evt);
        Task<IEnumerable<ESauceEvent>> StreamEvents(Guid aggregateId, int? lastVersion);
    }
}