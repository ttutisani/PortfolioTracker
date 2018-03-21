using PortfolioTracker.Core;
using StructureMap;

namespace PortfolioTracker.Infrastructure
{
    public sealed class InProcessEventManagerSource : IEventManagerSource
    {
        private readonly Container _container;

        public InProcessEventManagerSource(Container container)
        {
            _container = container ?? throw new System.ArgumentNullException(nameof(container));
        }

        public IEventManager Create()
        {
            return new InProcessEventManager(_container);
        }
    }
}
