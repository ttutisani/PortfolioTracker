namespace PortfolioTracker.AppServices
{
    public interface ICommandManagerSource
    {
        ICommandManager Create();
    }
}
