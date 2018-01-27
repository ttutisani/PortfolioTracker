using FluentAssertions;
using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using Xunit;

public sealed class PortfolioTests
{
    [Fact]
    public void Constructor_Initializes_All_Values()
    {
        //arrange.
        var id = Guid.NewGuid();
        var name = "name";
        var instrument = new Instrument("SYM", "name", 1);
        var holdings = new List<Holding> { new Holding(Guid.NewGuid(), instrument, new List<Lot> { new Lot(Guid.NewGuid(), DateTime.Now, instrument, 1) }) };
        var notes = "notes";

        //act.
        var sut = new Portfolio(id, name, holdings, notes);

        //assert.
        sut.Id.Should().Be(id);
        sut.Name.Should().Be(name);
        sut.Holdings.Should().Equal(holdings);
        sut.Notes.Should().Be(notes);
    }

    public static object[][] IsSameAs_TestData
    {
        get
        {
            var sut = new Portfolio(Guid.NewGuid(), "name");
            var clone = new Portfolio(sut.Id, sut.Name);
            var same = new Portfolio(sut.Id, "some other name");
            var different = new Portfolio(Guid.NewGuid(), sut.Name);

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
    public void IsSameAs_Compares_As_Entities(Portfolio sut, Portfolio other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }

    [Fact]
    public void RefreshPortfolio_Calculates_Based_On_Holdings()
    {
        //arrange.
        var instrument = new Instrument("SYM", "name", 1);
        var holding1 = new Holding(
            Guid.NewGuid(),
            instrument,
            new List<Lot> {
                new Lot(Guid.NewGuid(), DateTime.Now.AddDays(-1), instrument, 2),
                new Lot(Guid.NewGuid(), DateTime.Now.AddDays(-1), instrument, 2)
            });

        var holding2 = new Holding(
            Guid.NewGuid(),
            instrument,
            new List<Lot> { new Lot(Guid.NewGuid(), DateTime.Now.AddDays(-1), instrument, 2) });


        var sut = new Portfolio(Guid.NewGuid(), "name", new List<Holding> { holding1, holding2 });

        //act.
        sut.RefreshPerformance(DateTime.Now, 12, 6);
        var performance = sut.Performance;

        //assert.
        performance.Should().NotBeNull();

        performance.CostBasis.Should().NotBeNull();
        performance.CostBasis.Amount.Should().Be(holding1.Performance.CostBasis.Amount + holding2.Performance.CostBasis.Amount);
        performance.CostBasis.Percentage.Should().Be(50);

        performance.MarketValue.Should().NotBeNull();
        performance.MarketValue.Amount.Should().Be(holding1.Performance.MarketValue.Amount + holding2.Performance.MarketValue.Amount);
        performance.MarketValue.Percentage.Should().Be(50);

        var totalGain = performance.GetTotalGain();
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(performance.MarketValue.Amount - performance.CostBasis.Amount);
        totalGain.Percentage.Should().Be(totalGain.Amount / performance.CostBasis.Amount * 100);

        var annualGain = performance.GetAnnualGain();
        annualGain.Should().NotBeNull();
        annualGain.Amount.Should().Be(holding1.Performance.GetAnnualGain().Amount + holding2.Performance.GetAnnualGain().Amount);
        annualGain.Percentage.Should().Be(annualGain.Amount / performance.CostBasis.Amount * 100);
    }
}
