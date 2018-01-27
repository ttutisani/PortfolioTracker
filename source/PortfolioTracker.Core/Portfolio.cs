using PortfolioTracker.Core.Markers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PortfolioTracker.Core
{
    public sealed class Portfolio : IEntity, IAggregateRoot
    {
        public Portfolio(Guid id, string name, IList<Holding> holdings = null, string notes = null)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Holdings = new ReadOnlyCollection<Holding>(holdings ?? new List<Holding>());
            Notes = notes;
        }

        public Guid Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<Holding> Holdings { get; }

        public string Notes { get; }

        #region IEntity members

        public bool IsSameAs(IEntity other)
        {
            return other is Portfolio otherPortfolio
                ? Id == otherPortfolio.Id
                : false;
        }

        #endregion

        public void RefreshPerformance(
            DateTime now,
            decimal totalCostBasis, 
            decimal totalMarketValue)
        {
            var totalHoldingCostBasis = Holdings.Sum(holding => holding.GetPurchasePrice());
            var totalHoldingMarketValue = Holdings.Sum(holding => holding.GetCurrentPrice());

            foreach (var holding in Holdings)
            {
                holding.RefreshPerformance(
                    now, 
                    totalHoldingCostBasis, 
                    totalHoldingMarketValue);
            }

            Performance = new MoneyPerformanceIndicators(
                
                new AmountAndPercentage(totalHoldingCostBasis, totalHoldingCostBasis / totalCostBasis * 100),
                new AmountAndPercentage(totalHoldingMarketValue, totalHoldingMarketValue / totalMarketValue * 100),
                new MoneyPerformanceIndicators.AnnualGainCalculatorForPortfolio(Holdings)

                );
        }

        public MoneyPerformanceIndicators Performance { get; private set; }
    }
}
