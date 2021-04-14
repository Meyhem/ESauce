using System;

namespace ESauce.Model
{
    public abstract class AggregateBase
    {
        public AggregateBase()
        {
            AggregateId = Guid.NewGuid();
        }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

    }
}
