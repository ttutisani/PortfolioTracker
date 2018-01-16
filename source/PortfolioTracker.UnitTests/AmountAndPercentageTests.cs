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
}
