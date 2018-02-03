using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PortfolioTracker.Core.Markers;

namespace PortfolioTracker.Core
{
    public interface IHolding : IEntity
    {
        decimal GetPurchasePrice();
        decimal GetCurrentPrice();
        void RefreshPerformance(DateTime now, decimal totalHoldingCostBasis, decimal totalHoldingMarketValue);
        decimal GetAnnualGainAmount();
    }

    public sealed class Holding : IHolding
    {
        public Holding(
            Guid id, 
            Instrument instrument, 
            IList<ILot> lots = null, 
            string notes = null)
        {
            Id = id;
            Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));

            ValidateLots(instrument, lots);
            Lots = new ReadOnlyCollection<ILot>(lots ?? new List<ILot>());

            Notes = notes;
        }

        private static void ValidateLots(Instrument instrument, IList<ILot> lots)
        {
            if (lots == null)
                return;

            if (lots.Any(lot => !lot.IsForInstrument(instrument)))
                throw new InvalidOperationException("All lots should be for the instrument of this holding.");

            if (lots.Any(lot => lots.Count(otherLot => lot.IsSameAs(otherLot)) > 1))
                throw new InvalidOperationException("Lots cannot repeat.");
        }

        public Guid Id { get; }

        public Instrument Instrument { get; }

        public ReadOnlyCollection<ILot> Lots { get; }

        public string Notes { get; }

        #region IEntity members

        public bool IsSameAs(IEntity other)
        {
            return other is Holding otherHolding
                ? Id == otherHolding.Id
                : false;
        }

        #endregion IEntity members

        public void RefreshPerformance(
            DateTime now,
            decimal totalCostBasis,
            decimal totalMarketValue
            )
        {
            var totalLotCostBasis = Lots.Sum(lot => lot.PurchasePrice);
            var totalLotMarketValue = Lots.Sum(lot => lot.GetCurrentPrice());

            foreach (var lot in Lots)
            {
                lot.RefreshPerformance(now, totalLotCostBasis, totalLotMarketValue);
            }

            Performance = new MoneyPerformanceIndicators(
                new AmountAndPercentage(
                    totalLotCostBasis, 
                    totalLotCostBasis / totalCostBasis * 100),

                new AmountAndPercentage(
                    totalLotMarketValue, 
                    totalLotMarketValue / totalMarketValue * 100),

                new MoneyPerformanceIndicators.AnnualGainCalculatorForHolding(Lots)
                );
        }

        public MoneyPerformanceIndicators Performance { get; private set; }

        public decimal GetPurchasePrice()
        {
            return Lots.Sum(lot => lot.PurchasePrice);
        }

        public decimal GetCurrentPrice()
        {
            return Lots.Sum(lot => lot.GetCurrentPrice());
        }

        public decimal GetAnnualGainAmount()
        {
            return Performance.GetAnnualGain().Amount;
        }
    }
}
