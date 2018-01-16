using FluentAssertions;
using PortfolioTracker.Core;
using Xunit;

public sealed class LotTests
{
    [Fact]
    public void GetPerformance_Calculates_All_Info()
    {
        //arrange.
        var sut = new Lot(new System.DateTime(2000, 1, 1), new Instrument("TST", "Test symbol", 10m), 1m, null);

        //act.
        var performance = sut.GetPerformance(new System.DateTime(2000, 1, 2), 1m, 10m);

        //assert.
        performance.Should().NotBeNull();
        performance.DaysSincePurchase.Should().Be(1);

        performance.CostBasis.Should().NotBeNull();
        performance.CostBasis.Amount.Should().Be(sut.PurchasePrice);
        performance.CostBasis.Percentage.Should().Be(100m);

        performance.MarketValue.Should().NotBeNull();
        performance.MarketValue.Amount.Should().Be(10m);
        performance.MarketValue.Percentage.Should().Be(100m);
    }
}
