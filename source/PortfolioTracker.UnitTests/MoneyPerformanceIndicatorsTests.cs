using FluentAssertions;
using Moq;
using PortfolioTracker.Core;
using System;
using Xunit;

public sealed class MoneyPerformanceIndicatorsTests
{
    [Fact]
    public void Constructor_Requires_TotalToAnnualGainConverter()
    {
        //arrange.
        var construct = new Action(() => new MoneyPerformanceIndicators(new AmountAndPercentage(10, 10), new AmountAndPercentage(10, 10), null));

        //act / assert.
        construct.ShouldThrowExactly<ArgumentNullException>();
    }

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
            new AmountAndPercentage(costBasis, 100),
            new AmountAndPercentage(marketValue, 100),
            new Mock<MoneyPerformanceIndicators.IAnnualGainCalculator>().Object);

        //act.
        var totalGain = sut.GetTotalGain();

        //assert.
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(totalGainAmount);
        totalGain.Percentage.Should().Be(totalGainPercentage);
    }

    [Fact]
    public void GetAnnualGain_Calculates_Based_On_TotalGain_And_TotalToAnnualConverter()
    {
        //arrange.
        var expectedAnnualGain = new AmountAndPercentage(10, 10);
        var converter = new Mock<MoneyPerformanceIndicators.IAnnualGainCalculator>();
        converter.Setup(c => c.GetAnnualGain(It.IsAny<AmountAndPercentage>(), It.IsAny<AmountAndPercentage>()))
            .Returns(expectedAnnualGain);

        var sut = new MoneyPerformanceIndicators(
            new AmountAndPercentage(10, 100),
            new AmountAndPercentage(10, 100),
            converter.Object);

        //act.
        var annualGain = sut.GetAnnualGain();

        //assert.
        annualGain.Should().BeSameAs(expectedAnnualGain);
        converter.Verify(c => c.GetAnnualGain(It.Is<AmountAndPercentage>(aap => aap.IsSameAs(sut.CostBasis)), It.Is<AmountAndPercentage>(aap => aap.IsSameAs(sut.MarketValue))), Times.Once);
    }

    public static object[][] IsSameAs_TestData
    {
        get
        {
            var sut = new MoneyPerformanceIndicators(new AmountAndPercentage(1, 2), new AmountAndPercentage(3, 4), new Mock<MoneyPerformanceIndicators.IAnnualGainCalculator>().Object);
            var clone = new MoneyPerformanceIndicators(sut.CostBasis, sut.MarketValue, new Mock<MoneyPerformanceIndicators.IAnnualGainCalculator>().Object);
            var same = new MoneyPerformanceIndicators(new AmountAndPercentage(1, 2), new AmountAndPercentage(3, 4), new Mock<MoneyPerformanceIndicators.IAnnualGainCalculator>().Object);
            var different = new MoneyPerformanceIndicators(new AmountAndPercentage(4, 3), new AmountAndPercentage(2, 1), new Mock<MoneyPerformanceIndicators.IAnnualGainCalculator>().Object);

            return new[] 
            {
                new object[] { sut, sut, true },
                new object[] { sut, clone, true },
                new object[] { sut, same, true },
                new object[] { sut, different, false }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsSameAs_TestData))]
    public void IsSameAs_Compares_As_ValueObjects(MoneyPerformanceIndicators sut, MoneyPerformanceIndicators other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }
}
