namespace PortfolioTracker.AppServices
{
    public interface ICommandHandler<TCommand>
    {
        void Execute(TCommand command);
    }
}
