namespace PortfolioTracker.CLI.ArgumentHandling
{
    public interface IArgumentsHandler<TArguments>
    {
        void Handle(TArguments arguments);
    }
}
