using FluentAssertions;
using PortfolioTracker.Core;
using Xunit;

public sealed class MoneyPerformanceIndicatorsTests
{
    [Theory]
    [InlineData(1, 1, 0, 0)]
    [InlineData(1, 2, 1, 100)]
    [InlineData(1, 1.5, 0.5, 50)]
    [InlineData(1, 3, 2, 200)]
    public void GetTotalGain_Calculates_Based_On_CostBasis_And_MarketValue(
        decimal costBasis,
        decimal marketValue,
        decimal totalGainAmount,
        decimal totalGainPercentage
        )
    {
        //arrange.
        var sut = new MoneyPerformanceIndicators(
            10,
            new AmountAndPercentage(costBasis, 100),
            new AmountAndPercentage(marketValue, 100));

        //act.
        var totalGain = sut.GetTotalGain();

        //assert.
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(totalGainAmount);
        totalGain.Percentage.Should().Be(totalGainPercentage);
    }

    [Theory]
    [InlineData(10, 1, 2)]
    [InlineData(2, 1, 2)]
    [InlineData(50, 1, 2)]
    public void GetAnnualGain_Calculates_Based_On_TotalGain_And_DaysSincePurchase(
        int daysSincePurchase,
        decimal costBasis,
        decimal marketValue
        )
    {
        //arrange.
        var sut = new MoneyPerformanceIndicators(
            daysSincePurchase,
            new AmountAndPercentage(costBasis, 100),
            new AmountAndPercentage(marketValue, 100));

        //act.
        var annualGain = sut.GetAnnualGain();

        //assert.
        annualGain.Should().NotBeNull();

        var expectedAmount = (marketValue - costBasis) / daysSincePurchase * 365m;
        annualGain.Amount.Should().Be(expectedAmount);
        annualGain.Percentage.Should().Be(expectedAmount / costBasis * 100m);
    }
}
