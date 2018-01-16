using FluentAssertions;
using PortfolioTracker2.Core;
using System;
using Xunit;

public sealed class InstrumentTests
{
    [Fact]
    public void Constructor_Requires_All_Arguments()
    {
        //act / assert.
        new Action(() => new Instrument(null, "name 123", 123m)).ShouldThrow<ArgumentNullException>();
        new Action(() => new Instrument("SMB", null, 123m)).ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_Initializes_All_Properties()
    {
        //arrange.
        var expectedSymbol = "SMB";
        var expectedName = "name 123";
        var expectedPrice = 112121m;

        //act.
        var sut = new Instrument(expectedSymbol, expectedName, expectedPrice);

        //assert.
        sut.Symbol.Should().Be(expectedSymbol);
        sut.Name.Should().Be(expectedName);
        sut.CurrentPrice.Should().Be(expectedPrice);
    }
}