using FluentAssertions;
using Moq;
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
        var portfolios = new List<IPortfolio>
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
            var sut = new PortfolioGroup(Guid.NewGuid(), "name123", new List<IPortfolio> { new Portfolio(Guid.NewGuid(), "pname123") }, "notes123");
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

    [Fact]
    public void Performance_Is_Based_On_Portfolios()
    {
        //arrange.
        var portfolio1 = new Mock<IPortfolio>();
        portfolio1.Setup(p => p.GetPurchasePrice()).Returns(1);
        portfolio1.Setup(p => p.GetCurrentPrice()).Returns(3);

        var portfolio2 = new Mock<IPortfolio>();
        portfolio2.Setup(p => p.GetPurchasePrice()).Returns(2);
        portfolio2.Setup(p => p.GetCurrentPrice()).Returns(4);

        var sut = new PortfolioGroup(Guid.NewGuid(), "name123", new List<IPortfolio> { portfolio1.Object, portfolio2.Object });

        //act.
        sut.RefreshPerformance(DateTime.Now);
        var performance = sut.Performance;

        //assert.
        performance.Should().NotBeNull();

        performance.CostBasis.Should().NotBeNull();
        performance.CostBasis.Amount.Should().Be(portfolio1.Object.GetPurchasePrice() + portfolio2.Object.GetPurchasePrice());
        performance.CostBasis.Percentage.Should().Be(100);

        performance.MarketValue.Should().NotBeNull();
        performance.MarketValue.Amount.Should().Be(portfolio1.Object.GetCurrentPrice() + portfolio2.Object.GetCurrentPrice());
        performance.MarketValue.Percentage.Should().Be(100);

        var totalGain = performance.GetTotalGain();
        totalGain.Should().NotBeNull();
        totalGain.Amount.Should().Be(performance.MarketValue.Amount - performance.CostBasis.Amount);
        totalGain.Percentage.Should().Be(totalGain.Amount / performance.CostBasis.Amount * 100);

        var annualGain = performance.GetAnnualGain();
        annualGain.Should().NotBeNull();
        annualGain.Amount.Should().Be(portfolio1.Object.GetAnnualGainAmount() + portfolio2.Object.GetAnnualGainAmount());
        annualGain.Percentage.Should().Be(annualGain.Amount / performance.CostBasis.Amount * 100);
    }
}
