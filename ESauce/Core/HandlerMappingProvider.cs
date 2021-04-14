using System;
using System.Collections.Generic;
using ESauce.Model;

namespace ESauce.Core
{
    public class HandlerMappingProvider<TAggregate> : IHandlerMappingProvider<TAggregate> 
        where TAggregate : AggregateBase
    {
        private readonly Dictionary<string, Type> mapping = new();

        public void AddHandlerMapping<TEvent, TEventHandler>()
        {
            mapping.Add(typeof(TEvent).Name, typeof(TEventHandler));
        }

        public Type FindHandlerForEvent(string eventName)
        {
            return mapping[eventName];
        }
    }
}