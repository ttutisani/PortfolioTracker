using PortfolioTracker.Core;
using System;
using Xunit;

public sealed class InstrumentTests
{
    [Fact]
    public void Constructor_Requires_All_Arguments()
    {
        //act / assert.
        Assert.Throws<ArgumentNullException>(() => new Instrument(null, "name 123", 123m));
        Assert.Throws<ArgumentNullException>(() => new Instrument("SMB", null, 123m));
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
        Assert.Equal(expectedSymbol, sut.Symbol);
        Assert.Equal(expectedName, sut.Name);
        Assert.Equal(expectedPrice, sut.CurrentPrice);
    }
}