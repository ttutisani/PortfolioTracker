using System;

namespace PortfolioTracker.Core
{
    public interface IEventManager : IDisposable
    {
        void Raise(object domainEvent);
    }
}
