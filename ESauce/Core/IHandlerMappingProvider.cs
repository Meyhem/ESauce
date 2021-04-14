using System;
using ESauce.Model;

namespace ESauce.Core
{
    public interface IHandlerMappingProvider<TAggregate> where TAggregate : AggregateBase
    {
        void AddHandlerMapping<TEvent, TEventHandler>();
        Type FindHandlerForEvent(string eventName);
    }
}