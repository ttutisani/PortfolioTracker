using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortfolioTracker.Core
{
    public sealed class Instrument : Markers.IAggregateRoot
    {
        public Instrument(
            string symbol, 
            string name, 
            decimal currentPrice,
            List<Guid> lotIdList = null)
        {
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CurrentPrice = currentPrice;

            _lotIdList = lotIdList ?? new List<Guid>();
        }

        public string Symbol { get; }

        public string Name { get; }

        public decimal CurrentPrice { get; private set; }

        private readonly List<Guid> _lotIdList;
        public ReadOnlyCollection<Guid> LotIdList => _lotIdList.AsReadOnly();

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

        public void UpdatePrice(decimal newPrice, IEventManager eventManager)
        {
            if (newPrice <= 0)
                throw new ArgumentException($"`{nameof(newPrice)}' must be positive. Was `{newPrice}`.", nameof(newPrice));

            if (eventManager == null)
                throw new ArgumentNullException(nameof(eventManager));

            CurrentPrice = newPrice;
            eventManager.Raise(new InstrumentPriceChangedDomainEvent(Symbol));
        }
    }
}
