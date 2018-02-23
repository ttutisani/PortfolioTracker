namespace PortfolioTracker.Core
{
    public sealed class InstrumentInfo : Markers.IValueObject
    {
        public InstrumentInfo(string symbol, string name, decimal currentPrice)
        {
            Symbol = symbol ?? throw new System.ArgumentNullException(nameof(symbol));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            CurrentPrice = currentPrice;
        }

        public string Symbol { get; }

        public string Name { get; }

        public decimal CurrentPrice { get; }

        public bool IsSameAs(object other)
        {
            return other is InstrumentInfo ii
                ? (Symbol == ii.Symbol && Name == ii.Name && CurrentPrice == ii.CurrentPrice)
                : false;
        }
    }
}
