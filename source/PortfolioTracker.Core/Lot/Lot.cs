using System;

namespace PortfolioTracker.Core
{
    public sealed class Lot : Markers.IAggregateRoot
    {
        public Lot(
            Guid id, 
            InstrumentInfo instrumentInfo, 
            DateTime purchaseDate, 
            decimal purchasePrice, 
            string notes = null)
        {
            Id = id;
            InstrumentInfo = instrumentInfo ?? throw new ArgumentNullException(nameof(instrumentInfo));
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
            Notes = notes;
        }

        public Guid Id { get; }

        public InstrumentInfo InstrumentInfo { get; private set; }

        public DateTime PurchaseDate { get; }

        public decimal PurchasePrice { get; }

        public string Notes { get; set; }

        public bool IsSameAs(object other)
        {
            return other is Lot otherLot
                ? Id == otherLot.Id
                : false;
        }

        public void UpdateInstrumentPrice(decimal newPrice)
        {
            InstrumentInfo = new InstrumentInfo(
                InstrumentInfo.Symbol,
                InstrumentInfo.Name,
                newPrice);
        }
    }
}
