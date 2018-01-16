using System;

namespace PortfolioTracker2.Core
{
    public sealed class Lot : Markers.IEntity, Markers.IAggregateRoot
    {
        public Lot(DateTime purchaseDate, Instrument instrument, decimal purchasePrice, string notes)
        {
            PurchaseDate = purchaseDate;
            Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));
            PurchasePrice = purchasePrice;
            Notes = notes;
        }

        public DateTime PurchaseDate { get; }

        public Instrument Instrument { get; }

        public decimal PurchasePrice { get; }

        public string Notes { get; }

        public MoneyPerformanceIndicators GetPerformance(
            DateTime now, 
            decimal totalCostBasis, 
            decimal totalMarketValue)
        {
            var daysSincePurchase = now.Subtract(PurchaseDate).Days;
            var costBasis = new AmountAndPercentage(PurchasePrice, PurchasePrice / totalCostBasis * 100);
            var marketValue = new AmountAndPercentage(Instrument.CurrentPrice, Instrument.CurrentPrice / totalMarketValue * 100);

            return new MoneyPerformanceIndicators(daysSincePurchase, costBasis, marketValue);
        }
    }
}
