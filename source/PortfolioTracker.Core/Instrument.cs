namespace PortfolioTracker.Core
{
    public sealed class Instrument
    {
        public Instrument(string symbol, string name, decimal currentPrice)
        {
            Symbol = symbol ?? throw new System.ArgumentNullException(nameof(symbol));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            CurrentPrice = currentPrice;
        }

        public string Symbol { get; private set; }

        public string Name { get; private set; }

        public decimal CurrentPrice { get; private set; }
    }
}
