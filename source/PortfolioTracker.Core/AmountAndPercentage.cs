namespace PortfolioTracker.Core
{
    public sealed class AmountAndPercentage : Markers.IValueObject
    {
        public AmountAndPercentage(decimal amount, decimal percentage)
        {
            Amount = amount;
            Percentage = percentage;
        }

        public decimal Amount { get; private set; }

        public decimal Percentage { get; set; }
    }
}
