using PortfolioTracker.Core.Markers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioTracker.Core
{
    public sealed class MoneyPerformanceIndicators : IValueObject
    {
        #region annual gain calculation strategy

        public interface IAnnualGainCalculator
        {
            AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis,
                AmountAndPercentage marketValue);
        }

        public sealed class AnnualGainCalculatorForLot : IAnnualGainCalculator
        {
            private readonly DateTime _purchaseDate;
            private readonly DateTime _now;

            public AnnualGainCalculatorForLot(
                DateTime purchaseDate, 
                DateTime now)
            {
                _purchaseDate = purchaseDate;
                _now = now;
            }

            public AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis,
                AmountAndPercentage marketValue)
            {
                var totalGain = marketValue.Subtract(costBasis);

                var daysSincePurchase = _now.Subtract(_purchaseDate).Days;

                var annualGainAmount = daysSincePurchase > 0
                    ? totalGain.Amount / daysSincePurchase * Constants.DaysInYear
                    : 0;
                var annualGainPercentage = daysSincePurchase > 0
                    ? totalGain.Percentage / daysSincePurchase * Constants.DaysInYear
                    : 0;

                return new AmountAndPercentage(annualGainAmount, annualGainPercentage);
            }
        }

        public sealed class AnnualGainCalculatorForHolding : IAnnualGainCalculator
        {
            private readonly IEnumerable<ILot> _lots;

            public AnnualGainCalculatorForHolding(IEnumerable<ILot> lots)
            {
                _lots = lots ?? throw new ArgumentNullException(nameof(lots));
            }

            public AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis,
                AmountAndPercentage marketValue)
            {
                var annualGainAmount = _lots.Sum(lot => lot.GetAnnualGainAmount());
                var annualGainPercentage = annualGainAmount / costBasis.Amount * 100;

                var annualGain = new AmountAndPercentage(
                    annualGainAmount, 
                    annualGainPercentage);

                return annualGain;
            }
        }

        public sealed class AnnualGainCalculatorForPortfolio : IAnnualGainCalculator
        {
            private readonly IEnumerable<IHolding> _holdings;

            public AnnualGainCalculatorForPortfolio(IEnumerable<IHolding> holdings)
            {
                _holdings = holdings ?? throw new ArgumentNullException(nameof(holdings));
            }

            public AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis, 
                AmountAndPercentage marketValue)
            {
                var annualGainAmount = _holdings.Sum(holding => holding.GetAnnualGainAmount());
                var annualGainPercentage = annualGainAmount / costBasis.Amount * 100;

                return new AmountAndPercentage(annualGainAmount, annualGainPercentage);
            }
        }

        public sealed class AnnualGainCalculatorForPortfolioGroup : IAnnualGainCalculator
        {
            private readonly IEnumerable<IPortfolio> _portfolios;

            public AnnualGainCalculatorForPortfolioGroup(IEnumerable<IPortfolio> portfolios)
            {
                _portfolios = portfolios ?? throw new ArgumentNullException(nameof(portfolios));
            }

            public AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis,
                AmountAndPercentage marketValue)
            {
                var annualGainAmount = _portfolios.Sum(portfolio => portfolio.GetAnnualGainAmount());
                var annualGainPercentage = annualGainAmount / costBasis.Amount * 100;

                return new AmountAndPercentage(annualGainAmount, annualGainPercentage);
            }
        }

        #endregion

        private readonly IAnnualGainCalculator _annualGainCalculator;

        public MoneyPerformanceIndicators(
            AmountAndPercentage costBasis,
            AmountAndPercentage marketValue,
            IAnnualGainCalculator annualGainCalculator)
        {
            CostBasis = costBasis ?? throw new ArgumentNullException(nameof(costBasis));
            MarketValue = marketValue ?? throw new ArgumentNullException(nameof(marketValue));
            _annualGainCalculator = annualGainCalculator ?? throw new ArgumentNullException(nameof(annualGainCalculator));
        }

        public AmountAndPercentage CostBasis { get; }

        public AmountAndPercentage MarketValue { get; }

        #region IValueObject members

        public bool IsSameAs(IValueObject other)
        {
            return other is MoneyPerformanceIndicators otherPerformance
                ? CostBasis.IsSameAs(otherPerformance.CostBasis)
                    && MarketValue.IsSameAs(otherPerformance.MarketValue)
                : false;
        }

        #endregion

        public AmountAndPercentage GetTotalGain()
        {
            return MarketValue.Subtract(CostBasis);
        }

        public AmountAndPercentage GetAnnualGain()
        {
            var totalGain = GetTotalGain();

            return _annualGainCalculator.GetAnnualGain(
                CostBasis,
                MarketValue);
        }
    }
}
