using PortfolioTracker.Core.Markers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PortfolioTracker.Core
{
    public sealed class PortfolioGroup : IEntity, IAggregateRoot
    {
        public PortfolioGroup(Guid id, string name, IList<IPortfolio> portfolios = null, string notes = null)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Portfolios = new ReadOnlyCollection<IPortfolio>(portfolios ?? new List<IPortfolio>());
            Notes = notes;
        }

        public Guid Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<IPortfolio> Portfolios { get; }

        public string Notes { get; }

        #region IEntity members

        public bool IsSameAs(IEntity other)
        {
            return other is PortfolioGroup otherPortfolioGroup
                ? Id == otherPortfolioGroup.Id
                : false;
        }

        #endregion

        public void RefreshPerformance(DateTime now)
        {
            var totalPortfoliosCostBasis = Portfolios.Sum(p => p.GetPurchasePrice());
            var totalPortfoliosMarketValue = Portfolios.Sum(p => p.GetCurrentPrice());

            foreach (var portfolio in Portfolios)
            {
                portfolio.RefreshPerformance(now, totalPortfoliosCostBasis, totalPortfoliosMarketValue);
            }

            Performance = new MoneyPerformanceIndicators(
                
                new AmountAndPercentage(totalPortfoliosCostBasis, 100),
                new AmountAndPercentage(totalPortfoliosMarketValue, 100),
                new MoneyPerformanceIndicators.AnnualGainCalculatorForPortfolioGroup(Portfolios)

                );
        }

        public MoneyPerformanceIndicators Performance { get; private set; }
    }
}
