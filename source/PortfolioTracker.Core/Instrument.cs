using PortfolioTracker.Core.Markers;

namespace PortfolioTracker.Core
{
    public sealed class Instrument : IEntity, IAggregateRoot
    {
        public Instrument(string symbol, string name, decimal currentPrice)
        {
            Symbol = symbol ?? throw new System.ArgumentNullException(nameof(symbol));
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            CurrentPrice = currentPrice;
        }

        public string Symbol { get; }

        public string Name { get; }

        public decimal CurrentPrice { get; }

        #region IEntity members

        public bool IsSameAs(IEntity other)
        {
            return other is Instrument otherInstrument
                ? Symbol == otherInstrument.Symbol
                : false;
        }

        #endregion IEntity members
    }
}
