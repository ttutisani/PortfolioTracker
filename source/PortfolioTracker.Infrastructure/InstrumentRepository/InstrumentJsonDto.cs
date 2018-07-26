using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioTracker.Infrastructure
{
    internal sealed class InstrumentJsonDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public List<Guid> LotIdList { get; set; }

        public static InstrumentJsonDto FromInstrument(Instrument instrument)
        {
            return new InstrumentJsonDto
            {
                Symbol = instrument.Symbol,
                Name = instrument.Name,
                CurrentPrice = instrument.CurrentPrice,
                LotIdList = instrument.LotIdList.ToList()
            };
        }

        public Instrument ToInstrument()
        {
            return new Instrument(Symbol, Name, CurrentPrice, LotIdList);
        }
    }
}
