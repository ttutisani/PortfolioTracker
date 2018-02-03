using FluentAssertions;
using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using Xunit;

public sealed class PortfolioGroupTests
{
    [Fact]
    public void Constructor_Initializes_All_Values()
    {
        //arrange.
        var id = Guid.NewGuid();
        var name = "name123";
        var portfolios = new List<Portfolio>
        {
            new Portfolio(Guid.NewGuid(), "portfolio-name1"),
            new Portfolio(Guid.NewGuid(), "portfolio-name2")
        };
        var notes = "notes123";

        //act.
        var sut = new PortfolioGroup(id, name, portfolios, notes);

        //assert.
        sut.Id.Should().Be(id);
        sut.Name.Should().Be(name);
        sut.Portfolios.Should().Equal(portfolios);
        sut.Notes.Should().Be(notes);
    }

    public static object[][] IsSameAs_TestData
    {
        get
        {
            var sut = new PortfolioGroup(Guid.NewGuid(), "name123", new List<Portfolio> { new Portfolio(Guid.NewGuid(), "pname123") }, "notes123");
            var clone = new PortfolioGroup(sut.Id, sut.Name, sut.Portfolios, sut.Notes);
            var same = new PortfolioGroup(sut.Id, "other name");
            var different = new PortfolioGroup(Guid.NewGuid(), sut.Name, sut.Portfolios, sut.Notes);

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
    public void IsSameAs_Compares_As_Entities(PortfolioGroup sut, PortfolioGroup other, bool same)
    {
        //act.
        var isSame = sut.IsSameAs(other);

        //assert.
        isSame.Should().Be(same);
    }
}
