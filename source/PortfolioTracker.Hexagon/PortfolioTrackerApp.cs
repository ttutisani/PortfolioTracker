using PortfolioTracker.AppServices;
using StructureMap;
using System;

namespace PortfolioTracker.Hexagon
{
    public sealed class PortfolioTrackerApp
    {
        private readonly Container _container;

        internal PortfolioTrackerApp(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public ILotService GetLotService()
        {
            return _container.GetInstance<LotService>();
        }
    }
}
