using System;

namespace PortfolioTracker.Core
{
    public sealed class Lot : Markers.IAggregateRoot
    {
        public Lot(
            Guid id, 
            string instrumentSymbol, 
            DateTime purchaseDate, 
            decimal purchasePrice, 
            string notes = null)
        {
            Id = id;
            InstrumentSymbol = instrumentSymbol ?? throw new ArgumentNullException(nameof(instrumentSymbol));
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
            Notes = notes;
        }

        public Guid Id { get; }

        public string InstrumentSymbol { get; }

        public DateTime PurchaseDate { get; }

        public decimal PurchasePrice { get; set; }

        public string Notes { get; set; }

        public bool IsSameAs(object other)
        {
            return other is Lot otherLot
                ? Id == otherLot.Id
                : false;
        }
    }
}
