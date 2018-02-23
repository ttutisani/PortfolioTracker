namespace PortfolioTracker.Core
{
    public interface IEventManagerSource
    {
        IEventManager Create();
    }
}
