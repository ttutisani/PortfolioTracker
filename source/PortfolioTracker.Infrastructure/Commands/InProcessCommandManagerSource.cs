using PortfolioTracker.AppServices;
using StructureMap;

namespace PortfolioTracker.Infrastructure
{
    public sealed class InProcessCommandManagerSource : ICommandManagerSource
    {
        private readonly Container _container;

        public InProcessCommandManagerSource(Container container)
        {
            _container = container ?? throw new System.ArgumentNullException(nameof(container));
        }

        public ICommandManager Create()
        {
            return new InProcessCommandManager(_container);
        }
    }
}
