using FluentAssertions;
using Moq;
using PortfolioTracker.Core;
using PortfolioTracker.Core.Markers;
using System;
using System.Collections.Generic;
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

    public static object[][] IsSameAs_Test_Data
    {
        get
        {
            var sut = new Instrument("SYM", "name", 10m);

            return new []
            {
                new object[] { sut, sut, true },
                new object[] { sut, new Instrument("SYM", "other name", 100m), true },
                new object[] { sut, new Instrument("MYS", "name", 10m), false },
                new object[] { sut, null, false },
                new object[] { sut, new Mock<IEntity>().Object, false }
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsSameAs_Test_Data))]
    public void IsSameAs_Compares_As_Entities(Instrument sut, IEntity other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }
    
}