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

        public LotService GetLotService()
        {
            return _container.GetInstance<LotService>();
        }

        public InstrumentService GetInstrumentService()
        {
            return _container.GetInstance<InstrumentService>();
        }
    }
}
