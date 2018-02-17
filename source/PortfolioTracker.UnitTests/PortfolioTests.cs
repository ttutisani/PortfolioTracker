using FluentAssertions;
using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class PortfolioTests
    {
        [Fact]
        public void Constructor_Initializes_All_Values()
        {
            //arrange.
            var id = Guid.NewGuid();
            var name = "name";
            var holdings = new List<Holding> { new Holding(Guid.NewGuid(), "SYM", new List<Guid> { Guid.NewGuid() }) };
            var notes = "notes";

            //act.
            var sut = new Portfolio(id, name, holdings, notes);

            //assert.
            sut.Id.Should().Be(id);
            sut.Name.Should().Be(name);
            sut.Holdings.Should().Equal(holdings);
            sut.Notes.Should().Be(notes);
        }

        [Fact]
        public void Constructor_Initializes_EmptyHoldings_If_None_Passed()
        {
            //arrange / act.
            var sut = new Portfolio(Guid.NewGuid(), "name");

            //assert.
            sut.Holdings.Should().NotBeNull();
            sut.Holdings.Should().BeEmpty();
        }

        public static object[][] IsSameAs_TestData
        {
            get
            {
                var sut = new Portfolio(Guid.NewGuid(), "name");
                var clone = new Portfolio(sut.Id, sut.Name, sut.Holdings, sut.Notes);
                var same = new Portfolio(sut.Id, "some other name");
                var different = new Portfolio(Guid.NewGuid(), sut.Name);
                var veryDifferent = new object();

                return new[]
                {
                    new object[] { sut, sut, true },
                    new object[] { sut, clone, true },
                    new object[] { sut, same, true },
                    new object[] { sut, different, false },
                    new object[] { sut, veryDifferent, false },
                    new object[] { sut, null, false }
                };
            }
        }

        [Theory]
        [MemberData(nameof(IsSameAs_TestData))]
        public void IsSameAs_Compares_As_Entities(Portfolio sut, object other, bool same)
        {
            //act.
            var isSame = sut.IsSameAs(other);

            //assert.
            isSame.Should().Be(same);
        }
    }
}
