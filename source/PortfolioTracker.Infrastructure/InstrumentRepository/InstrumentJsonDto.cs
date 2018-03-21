using System;
using PortfolioTracker.Core;

namespace PortfolioTracker.Infrastructure
{
    internal sealed class InstrumentJsonDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }

        public static InstrumentJsonDto FromInstrument(Instrument instrument)
        {
            return new InstrumentJsonDto
            {
                Symbol = instrument.Symbol,
                Name = instrument.Name,
                CurrentPrice = instrument.CurrentPrice
            };
        }

        public Instrument ToInstrument()
        {
            return new Instrument(Symbol, Name, CurrentPrice);
        }
    }
}
