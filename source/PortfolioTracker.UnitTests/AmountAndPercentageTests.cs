using FluentAssertions;
using PortfolioTracker.Core;
using Xunit;

public sealed class AmountAndPercentageTests
{
    [Fact]
    public void Constructor_Initializes_All_Properties()
    {
        //arrange.
        var expectedAmount = 123m;
        var expectedPercentage = 321m;

        //act.
        var sut = new AmountAndPercentage(expectedAmount, expectedPercentage);

        //assert.
        sut.Amount.Should().Be(expectedAmount);
        sut.Percentage.Should().Be(expectedPercentage);
    }

    public static object[][] IsSameAs_TestData
    {
        get
        {
            var sut = new AmountAndPercentage(1, 2);
            var clone = new AmountAndPercentage(sut.Amount, sut.Percentage);
            var different = new AmountAndPercentage(2, 1);

            return new[]
            {
                new object[] { sut, sut, true },
                new object[] { sut, clone, true },
                new object[] { sut, different, false }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsSameAs_TestData))]
    public void IsSameAs_Compares_As_ValueObjects(AmountAndPercentage sut, AmountAndPercentage other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }

    [Fact]
    public void Subtract_Calculates_TotalGain()
    {
        //arrange.
        var sut = new AmountAndPercentage(100, 100);
        var other = new AmountAndPercentage(10, 100);

        //act.
        var totalGain = sut.Subtract(other);

        //assert.
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(90);
        totalGain.Percentage.Should().Be(900); //(100 - 10) / 10 * 100.
    }
}
