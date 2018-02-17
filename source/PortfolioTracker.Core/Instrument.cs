namespace PortfolioTracker.Core
{
    public sealed class Instrument : Markers.IAggregateRoot
    {
        public Instrument(string symbol, string name, decimal currentPrice)
        {
            Symbol = symbol ?? throw new System.ArgumentNullException(nameof(symbol));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            CurrentPrice = currentPrice;
        }

        public string Symbol { get; }

        public string Name { get; }

        public decimal CurrentPrice { get; set; }

        public bool IsSameAs(object other)
        {
            return other is Instrument otherInstrument
                ? Symbol == otherInstrument.Symbol
                : false;
        }
    }
}
