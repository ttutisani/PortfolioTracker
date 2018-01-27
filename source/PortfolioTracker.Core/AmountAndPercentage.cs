using PortfolioTracker.Core.Markers;
using System;

namespace PortfolioTracker.Core
{
    public sealed class AmountAndPercentage : IValueObject
    {
        public AmountAndPercentage(decimal amount, decimal percentage)
        {
            Amount = amount;
            Percentage = percentage;
        }

        public decimal Amount { get; }

        public decimal Percentage { get; }

        #region IValueObject members

        public bool IsSameAs(IValueObject other)
        {
            return other is AmountAndPercentage otherAmountAndPercentage
                ? Amount == otherAmountAndPercentage.Amount 
                    && Percentage == otherAmountAndPercentage.Percentage
                : false;
        }

        #endregion IValueObject members

        public AmountAndPercentage Subtract(AmountAndPercentage other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var gainAmount = Amount - other.Amount;
            var gainPercentage = gainAmount / other.Amount * 100;

            return new AmountAndPercentage(gainAmount, gainPercentage);
        }
    }
}
