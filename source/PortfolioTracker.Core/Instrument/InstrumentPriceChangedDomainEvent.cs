using System;

namespace PortfolioTracker.Core
{
    public sealed class InstrumentPriceChangedDomainEvent
    {
        public InstrumentPriceChangedDomainEvent(string instrumentSymbol)
        {
            InstrumentSymbol = !string.IsNullOrWhiteSpace(instrumentSymbol) 
                ? instrumentSymbol 
                : throw new ArgumentNullException(instrumentSymbol);
        }

        public string InstrumentSymbol { get; }
    }
}
