using System;

namespace PortfolioTracker.AppServices
{
    public interface ICommandManager : IDisposable
    {
        void Send(object command);
    }
}
