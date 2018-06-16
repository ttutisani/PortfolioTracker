using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortfolioTracker.Core
{
    public sealed class Instrument : Markers.IAggregateRoot
    {
        public Instrument(string symbol, string name, decimal currentPrice)
        {
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CurrentPrice = currentPrice;
        }

        public string Symbol { get; }

        public string Name { get; }

        public decimal CurrentPrice { get; set; }

        private readonly List<Guid> _lotIdList = new List<Guid>();
        public ReadOnlyCollection<Guid> LotIdList => new ReadOnlyCollection<Guid>(_lotIdList);

        public bool IsSameAs(object other)
        {
            return other is Instrument otherInstrument
                ? Symbol == otherInstrument.Symbol
                : false;
        }

        public void AttachLotId(Guid id)
        {
            _lotIdList.Add(id);
        }
    }
}
