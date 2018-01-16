namespace PortfolioTracker2.Core
{
    public sealed class MoneyPerformanceIndicators : Markers.IValueObject
    {
        private const int _daysInYear = 365;

        public MoneyPerformanceIndicators(
            int daysSincePurchase,
            AmountAndPercentage costBasis,
            AmountAndPercentage marketValue)
        {
            DaysSincePurchase = daysSincePurchase;
            CostBasis = costBasis ?? throw new System.ArgumentNullException(nameof(costBasis));
            MarketValue = marketValue ?? throw new System.ArgumentNullException(nameof(marketValue));
        }

        public int DaysSincePurchase { get; }

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

            var annualGainAmount = totalGain.Amount / DaysSincePurchase * _daysInYear;
            var annualGainPercentage = totalGain.Percentage / DaysSincePurchase * _daysInYear;

            return new AmountAndPercentage(annualGainAmount, annualGainPercentage);
        }
    }
}
