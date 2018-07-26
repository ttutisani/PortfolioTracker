namespace PortfolioTracker.AppServices
{
    public interface IInstrumentService
    {
        void UpdateInstrumentPrice(string symbol, decimal newPrice);
    }
}