using FluentAssertions;
using PortfolioTracker.Core;
using System;
using Xunit;

public sealed class LotTests
{
    [Fact]
    public void Constructor_Initializes_All_Values()
    {
        //arrange.
        var id = Guid.NewGuid();
        var purchaseDate = DateTime.Now;
        var instrument = new Instrument("SYM", "name", 10m);
        var purchasePrice = 1m;
        var notes = "some notes";
        
        //act.
        var sut = new Lot(id, purchaseDate, instrument, purchasePrice, notes);

        //assert.
        sut.PurchaseDate.Should().Be(purchaseDate);
        sut.Instrument.Should().BeSameAs(instrument);
        sut.PurchasePrice.Should().Be(purchasePrice);
        sut.Notes.Should().Be(notes);
        sut.Id.Should().Be(id);
    }

    public static object[][] IsSameAs_TestData
    {
        get
        {
            var sut = new Lot(Guid.NewGuid(), DateTime.Now, new Instrument("SYM", "name", 10m), 10m);
            var clone = new Lot(sut.Id, sut.PurchaseDate, sut.Instrument, sut.PurchasePrice);
            var same = new Lot(sut.Id, DateTime.Now.AddDays(-1), new Instrument("ANY", "name", 100m), 100m);
            var notSame = new Lot(Guid.NewGuid(), sut.PurchaseDate, sut.Instrument, sut.PurchasePrice);
            var different = new Lot(Guid.NewGuid(), DateTime.Now.AddDays(-10), new Instrument("ANY", "name", 123m), 123m);

            return new[]
            {
                new object[] { sut, sut, true },
                new object[] { sut, clone, true },
                new object[] { sut, same, true },
                new object[] { sut, notSame, false },
                new object[] { sut, different, false }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsSameAs_TestData))]
    public void IsSameAs_Compares_As_Entities(Lot sut, Lot other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }

    [Fact]
    public void GetPerformance_Calculates_All_Info()
    {
        //arrange.
        var sut = new Lot(Guid.NewGuid(), new DateTime(2000, 1, 1), new Instrument("TST", "Test symbol", 10m), 1m, null);

        //act.
        sut.RefreshPerformance(new DateTime(2000, 1, 2), 2m, 20m);
        var performance = sut.Performance;

        //assert.
        performance.Should().NotBeNull();

        performance.CostBasis.Should().NotBeNull();
        performance.CostBasis.Amount.Should().Be(sut.PurchasePrice);
        performance.CostBasis.Percentage.Should().Be(50m);

        performance.MarketValue.Should().NotBeNull();
        performance.MarketValue.Amount.Should().Be(10m);
        performance.MarketValue.Percentage.Should().Be(50m);

        var totalGain = performance.GetTotalGain();
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(performance.MarketValue.Amount - performance.CostBasis.Amount);
        totalGain.Percentage.Should().Be(totalGain.Amount / performance.CostBasis.Amount * 100);

        var annualGain = performance.GetAnnualGain();
        annualGain.Should().NotBeNull();
        annualGain.Amount.Should().Be((10m - 1m) * Constants.DaysInYear);
        annualGain.Percentage.Should().Be(annualGain.Amount / performance.CostBasis.Amount * 100);
    }
}
