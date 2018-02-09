using System;
using PortfolioTracker.Core.Markers;

namespace PortfolioTracker.Core
{
    public interface ILot : IEntity, IAggregateRoot
    {
        bool IsForInstrument(IInstrument instrument);
        decimal PurchasePrice { get; }
        decimal GetCurrentPrice();
        void RefreshPerformance(DateTime now, decimal totalLotCostBasis, decimal totalLotMarketValue);
        decimal GetAnnualGainAmount();
    }

    public sealed class Lot : ILot
    {
        public Lot(
            Guid id, 
            DateTime purchaseDate, 
            IInstrument instrument, 
            decimal purchasePrice, 
            string notes = null)
        {
            Id = id;
            PurchaseDate = purchaseDate;
            Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));
            PurchasePrice = purchasePrice;
            Notes = notes;
        }

        public Guid Id { get; }

        public DateTime PurchaseDate { get; }

        public IInstrument Instrument { get; }

        public decimal PurchasePrice { get; }

        public string Notes { get; }

        #region IEntity members

        public bool IsSameAs(IEntity other)
        {
            return other is Lot otherLot
                ? Id == otherLot.Id
                : false;
        }

        #endregion

        public decimal GetAnnualGainAmount()
        {
            return Performance.GetAnnualGain().Amount;
        }

        public void RefreshPerformance(
            DateTime now, 
            decimal totalCostBasis, 
            decimal totalMarketValue)
        {
            var costBasis = new AmountAndPercentage(
                PurchasePrice, 
                PurchasePrice / totalCostBasis * 100);

            var marketValue = new AmountAndPercentage(
                Instrument.CurrentPrice, 
                Instrument.CurrentPrice / totalMarketValue * 100);

            Performance = new MoneyPerformanceIndicators(
                costBasis, 
                marketValue, 
                new MoneyPerformanceIndicators.AnnualGainCalculatorForLot(PurchaseDate, now));
        }

        public MoneyPerformanceIndicators Performance { get; private set; }

        public bool IsForInstrument(IInstrument instrument)
        {
            return Instrument.IsSameAs(instrument);
        }

        public decimal GetCurrentPrice()
        {
            return Instrument.CurrentPrice;
        }
    }
}
