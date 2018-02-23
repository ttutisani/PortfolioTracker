using System;

namespace PortfolioTracker.Core
{
    public sealed class LotWasCreatedDomainEvent
    {
        public LotWasCreatedDomainEvent(Guid lotId)
        {
            LotId = lotId;
        }

        public Guid LotId { get; }
    }
}
