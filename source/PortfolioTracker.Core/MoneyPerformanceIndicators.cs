using PortfolioTracker.Core.Markers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioTracker.Core
{
    public sealed class MoneyPerformanceIndicators : IValueObject
    {
        #region Total to annual conversion strategy

        public interface IAnnualGainCalculator
        {
            AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis,
                AmountAndPercentage martketValue,
                AmountAndPercentage totalGain
                );
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
                AmountAndPercentage martketValue,
                AmountAndPercentage totalGain
                )
            {
                var daysSincePurchase = _now.Subtract(_purchaseDate).Days;

                var annualGainAmount = totalGain.Amount / daysSincePurchase * Constants.DaysInYear;
                var annualGainPercentage = totalGain.Percentage / daysSincePurchase * Constants.DaysInYear;

                return new AmountAndPercentage(annualGainAmount, annualGainPercentage);
            }
        }

        public sealed class AnnualGainCalculatorForHolding : IAnnualGainCalculator
        {
            private readonly IEnumerable<Lot> _lots;

            public AnnualGainCalculatorForHolding(IEnumerable<Lot> lots)
            {
                _lots = lots ?? throw new ArgumentNullException(nameof(lots));
            }

            public AmountAndPercentage GetAnnualGain(
                AmountAndPercentage costBasis,
                AmountAndPercentage martketValue,
                AmountAndPercentage totalGain
                )
            {
                var annualGainAmount = _lots.Sum(lot => lot.GetAnnualGainAmount());
                var annualGainPercentage = annualGainAmount / costBasis.Amount * 100;

                var annualGain = new AmountAndPercentage(
                    annualGainAmount, 
                    annualGainPercentage);

                return annualGain;
            }
        }

        #endregion

        private readonly IAnnualGainCalculator _totalToAnnualGain;

        public MoneyPerformanceIndicators(
            AmountAndPercentage costBasis,
            AmountAndPercentage marketValue,
            IAnnualGainCalculator totalToAnnualGain)
        {
            CostBasis = costBasis ?? throw new ArgumentNullException(nameof(costBasis));
            MarketValue = marketValue ?? throw new ArgumentNullException(nameof(marketValue));
            _totalToAnnualGain = totalToAnnualGain ?? throw new ArgumentNullException(nameof(totalToAnnualGain));
        }

        public AmountAndPercentage CostBasis { get; }

        public AmountAndPercentage MarketValue { get; }

        public AmountAndPercentage GetTotalGain()
        {
            var totalGainAmount = MarketValue.Amount - CostBasis.Amount;
            var totalGainPercentage = totalGainAmount / CostBasis.Amount * 100;

            return new AmountAndPercentage(totalGainAmount, totalGainPercentage);
        }

        public AmountAndPercentage GetAnnualGain()
        {
            var totalGain = GetTotalGain();

            return _totalToAnnualGain.GetAnnualGain(
                CostBasis, 
                MarketValue, 
                totalGain);
        }

        #region IValueObject members

        public bool IsSameAs(IValueObject other)
        {
            return other is MoneyPerformanceIndicators otherPerformance
                ? CostBasis.IsSameAs(otherPerformance.CostBasis) 
                    && MarketValue.IsSameAs(otherPerformance.MarketValue)
                : false;
        }

        #endregion
    }
}
