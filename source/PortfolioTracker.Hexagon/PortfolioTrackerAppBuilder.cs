using PortfolioTracker.Core;
using StructureMap;

namespace PortfolioTracker.Hexagon
{
    /// <summary>
    /// Factory for App.
    /// Exists because we can allow replacing interface registrations befor the app usage, but NOT after.
    /// Once the App is constructed and used at least once, replacing any registration can be dangerous,
    /// because it may already be used by dependencies, either inside the same IoC or outside by consumers.
    /// </summary>
    public sealed class PortfolioTrackerAppBuilder
    {
        private readonly Container _container = ContainerBuilder.BuildContainer();

        public ILotRepository LotRepository
        {
            get
            {
                return _container.GetInstance<ILotRepository>();
            }
            set
            {
                _container.Configure(_ => 
                {
                    _.For<ILotRepository>().Use(value);
                });
            }
        }

        public IInstrumentRepository InstrumentRepository
        {
            get
            {
                return _container.GetInstance<IInstrumentRepository>();
            }
            set
            {
                _container.Configure(_ =>
                {
                    _.For<IInstrumentRepository>().Use(value);
                });
            }
        }

        public PortfolioTrackerApp BuildApp()
        {
            return new PortfolioTrackerApp(_container);
        }
    }
}
