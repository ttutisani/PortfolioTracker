using StructureMap;

namespace PortfolioTracker.CLI.IoC
{
    public static class StructureMapIoC
    {
        static StructureMapIoC()
        {
            var container = new Container();

            var portfolioTrackerApp = new Hexagon.PortfolioTrackerAppBuilder().BuildApp();
            container.Configure(_ => _.For<AppServices.ILotService>().Use(ctx => portfolioTrackerApp.GetLotService()));
            container.Configure(_ => _.For<AppServices.IInstrumentService>().Use(ctx => portfolioTrackerApp.GetInstrumentService()));

            Container = container;
        }

        public static Container Container { get; }
    }
}
