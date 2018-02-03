using FluentAssertions;
using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using Xunit;

public sealed class HoldingTests
{
    [Fact]
    public void Constructor_Initializes_All_Values()
    {
        //arrange.
        var id = Guid.NewGuid();
        var instrument = new Instrument("SYM", "name", 10m);
        var lots = new List<ILot>() { new Lot(Guid.NewGuid(), DateTime.Now, instrument, 1m) };
        var notes = "some notes 123";

        //act.
        var sut = new Holding(id, instrument, lots, notes);

        //assert.
        sut.Id.Should().Be(id);
        sut.Instrument.Should().BeSameAs(instrument);
        sut.Lots.Should().Equal(lots);
        sut.Notes.Should().Be(notes);
    }

    [Fact]
    public void Constructor_Initializes_Empty_Lots_If_Not_Passed()
    {
        //arrange.
        var instrument = new Instrument("SYM", "name", 10m);

        //act.
        var sut = new Holding(Guid.NewGuid(), new Instrument("SYM", "name", 10m));

        //assert.
        sut.Lots.Should().NotBeNull();
        sut.Lots.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_Does_Not_Accept_Lot_With_Instrument_Other_Than_Holdings_Instrument()
    {
        //arrange.
        var instrument = new Instrument("SYM", "name", 10m);
        var badLot = new Lot(Guid.NewGuid(), DateTime.Now, new Instrument("SOM", "name", 10m), 10m, "some other notes");
        
        var construct = new Action(() => new Holding(Guid.NewGuid(), instrument, new List<ILot> { badLot }));

        //act / assert.
        construct.ShouldThrowExactly<InvalidOperationException>();
    }
    
    [Fact]
    public void Constructor_Does_Not_Accept_SameLot_Twice()
    {
        //arrange.
        var instrument = new Instrument("SYM", "name", 10m);
        var lot = new Lot(Guid.NewGuid(), DateTime.Now, instrument, 10m);

        var construct = new Action(() => new Holding(Guid.NewGuid(), instrument, new List<ILot> { lot, lot }));

        //act / assert.
        construct.ShouldThrowExactly<InvalidOperationException>();
    }

    public static object[][] IsSameAs_TestData
    {
        get
        {
            var sut = new Holding(Guid.NewGuid(), new Instrument("SYM", "name", 10m));
            var clone = new Holding(sut.Id, sut.Instrument, sut.Lots, sut.Notes);
            var same = new Holding(sut.Id, new Instrument("ANY", "name 123", 100m));
            var notSame = new Holding(Guid.NewGuid(), sut.Instrument, sut.Lots, sut.Notes);
            var different = new Holding(Guid.NewGuid(), new Instrument("ANY", "name 123", 100m));

            return new [] 
            {
                new object[] { sut, sut, true },
                new object[] { sut, clone, true },
                new object[] { sut, same, true },
                new object[] { sut, notSame, false },
                new object[] { sut, notSame, false }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsSameAs_TestData))]
    public void IsSameAs_Compares_As_Entities(Holding sut, Holding other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }

    [Fact]
    public void GetPerformance_Calculates_Holding_Performance_Based_On_Lots()
    {
        //arrange.
        var instrument = new Instrument("SYM", "some name", 4m);

        var lot1 = new Lot(Guid.NewGuid(), DateTime.Now.AddDays(-1), instrument, 1m);
        var lot2 = new Lot(Guid.NewGuid(), DateTime.Now.AddDays(-2), instrument, 2m);

        var sut = new Holding(Guid.NewGuid(), instrument, new List<ILot> { lot1, lot2 });

        //act.
        sut.RefreshPerformance(DateTime.Now, 6m, 16m);
        var performance = sut.Performance;

        //assert.
        performance.Should().NotBeNull();

        performance.CostBasis.Should().NotBeNull();
        performance.CostBasis.Amount.Should().Be(3m);
        performance.CostBasis.Percentage.Should().Be(50m);

        performance.MarketValue.Should().NotBeNull();
        performance.MarketValue.Amount.Should().Be(8m);
        performance.MarketValue.Percentage.Should().Be(50m);

        var totalGain = performance.GetTotalGain();
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(performance.MarketValue.Amount - performance.CostBasis.Amount);
        totalGain.Percentage.Should().Be(totalGain.Amount / performance.CostBasis.Amount * 100);

        var annualGain = performance.GetAnnualGain();
        annualGain.Should().NotBeNull();
        annualGain.Amount.Should().Be(lot1.Performance.GetAnnualGain().Amount + lot2.Performance.GetAnnualGain().Amount);
        annualGain.Percentage.Should().Be(annualGain.Amount / performance.CostBasis.Amount * 100);
    }
}
