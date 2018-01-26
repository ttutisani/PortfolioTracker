using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PortfolioTracker.Core.Markers;

namespace PortfolioTracker.Core
{
    public sealed class Holding : IEntity
    {
        public Holding(
            Guid id, 
            Instrument instrument, 
            IList<Lot> lots = null, 
            string notes = null)
        {
            Id = id;
            Instrument = instrument ?? throw new ArgumentNullException(nameof(instrument));

            ValidateLots(instrument, lots);
            Lots = new ReadOnlyCollection<Lot>(lots ?? new List<Lot>());

            Notes = notes;
        }

        private static void ValidateLots(Instrument instrument, IList<Lot> lots)
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

        public ReadOnlyCollection<Lot> Lots { get; }

        public string Notes { get; }

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

        #region IEntity members

        public bool IsSameAs(IEntity other)
        {
            return other is Holding otherHolding
                ? Id == otherHolding.Id
                : false;
        }

        #endregion IEntity members
    }
}
