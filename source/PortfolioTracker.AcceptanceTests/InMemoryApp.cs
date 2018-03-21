namespace PortfolioTracker.AcceptanceTests
{
    public sealed class InMemoryApp
    {
        public Hexagon.PortfolioTrackerApp PortfolioTrackerApp { get; }

        public Stubs.InMemoryInstrumentRepository InstrumentRepository { get; } = new Stubs.InMemoryInstrumentRepository();

        public Stubs.InMemoryLotRepository LotRepository { get; } = new Stubs.InMemoryLotRepository();

        public InMemoryApp()
        {
            var appBuilder = new Hexagon.PortfolioTrackerAppBuilder();
            appBuilder.InstrumentRepository = InstrumentRepository;
            appBuilder.LotRepository = LotRepository;

            PortfolioTrackerApp = appBuilder.BuildApp();
        }
    }
}
