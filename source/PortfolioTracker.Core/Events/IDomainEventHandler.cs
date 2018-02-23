namespace PortfolioTracker.Core
{
    public interface IDomainEventHandler<TDomainEvent>
    {
        void When(TDomainEvent domainEvent);
    }
}
