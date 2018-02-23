namespace PortfolioTracker.Core.Markers
{
    public interface IRepository<TAggregateRoot, TId>
        where TAggregateRoot: IAggregateRoot
    {
        void Add(TAggregateRoot aggregateRoot);

        TAggregateRoot GetById(TId id);

        void Update(TAggregateRoot aggregateRoot);
    }
}
