using FluentAssertions;
using PortfolioTracker.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class HoldingTests
    {
        [Fact]
        public void Constructor_Initializes_All_Values()
        {
            //arrange.
            var id = Guid.NewGuid();
            var instrumentSymbol = "SYM";
            var lotIdList = new List<Guid>() { Guid.NewGuid() };
            var notes = "some notes 123";

            //act.
            var sut = new Holding(id, instrumentSymbol, lotIdList, notes);

            //assert.
            sut.Id.Should().Be(id);
            sut.InstrumentSymbol.Should().Be(instrumentSymbol);
            sut.LotIdList.Should().Equal(lotIdList);
            sut.Notes.Should().Be(notes);
        }

        [Fact]
        public void Constructor_Initializes_Empty_Lots_If_Not_Passed()
        {
            //arrange.
            var instrument = new Instrument("SYM", "name", 10m);

            //act.
            var sut = new Holding(Guid.NewGuid(), "SYM");

            //assert.
            sut.LotIdList.Should().NotBeNull();
            sut.LotIdList.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_Does_Not_Accept_SameLot_Twice()
        {
            //arrange.
            var lotId = Guid.NewGuid();

            var construct = new Action(() => new Holding(Guid.NewGuid(), "SYM", new List<Guid> { lotId, lotId }));

            //act / assert.
            construct.ShouldThrowExactly<InvalidOperationException>();
        }

        public static object[][] IsSameAs_TestData
        {
            get
            {
                var sut = new Holding(Guid.NewGuid(), "SYM");
                var clone = new Holding(sut.Id, sut.InstrumentSymbol, sut.LotIdList, sut.Notes);
                var same = new Holding(sut.Id, "ANY");
                var notSame = new Holding(Guid.NewGuid(), sut.InstrumentSymbol, sut.LotIdList, sut.Notes);
                var different = new Holding(Guid.NewGuid(), "ANY");
                var veryDifferent = new object();

                return new[]
                {
                    new object[] { sut, sut, true },
                    new object[] { sut, clone, true },
                    new object[] { sut, same, true },
                    new object[] { sut, notSame, false },
                    new object[] { sut, notSame, false },
                    new object[] { sut, veryDifferent, false }
                };
            }
        }

        [Theory]
        [MemberData(nameof(IsSameAs_TestData))]
        public void IsSameAs_Compares_As_Entities(Holding sut, object other, bool same)
        {
            //act.
            var isSame = sut.IsSameAs(other);

            //assert.
            isSame.Should().Be(same);
        }
    }
}
