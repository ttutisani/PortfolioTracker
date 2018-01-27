using FluentAssertions;
using PortfolioTracker.Core;
using System;
using Xunit;
using static PortfolioTracker.Core.MoneyPerformanceIndicators;

public sealed class AnnualGainCalculatorForLotTests
{
    [Fact]
    public void GetAnnualGain_Gives_Zeros_For_SameDatePurchase()
    {
        //arrange.
        var sut = new AnnualGainCalculatorForLot(DateTime.Today, DateTime.Now);

        //act.
        var annualGain = sut.GetAnnualGain(new AmountAndPercentage(1, 1), new AmountAndPercentage(2, 2));

        //assert.
        annualGain.Should().NotBeNull();
        annualGain.Amount.Should().Be(0);
        annualGain.Percentage.Should().Be(0);
    }
}
