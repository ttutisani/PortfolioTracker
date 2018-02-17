using FluentAssertions;
using PortfolioTracker.Core;
using System;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class LotTests
    {
        [Fact]
        public void Constructor_Initializes_All_Values()
        {
            //arrange.
            var id = Guid.NewGuid();
            var purchaseDate = DateTime.Now;
            var instrumentSymbol = "SYM";
            var purchasePrice = 1m;
            var notes = "some notes";

            //act.
            var sut = new Lot(id, instrumentSymbol, purchaseDate, purchasePrice, notes);

            //assert.
            sut.Id.Should().Be(id);
            sut.PurchaseDate.Should().Be(purchaseDate);
            sut.InstrumentSymbol.Should().Be(instrumentSymbol);
            sut.PurchasePrice.Should().Be(purchasePrice);
            sut.Notes.Should().Be(notes);
        }

        public static object[][] IsSameAs_TestData
        {
            get
            {
                var sut = new Lot(Guid.NewGuid(), "SYM", DateTime.Now, 10m);
                var clone = new Lot(sut.Id, sut.InstrumentSymbol, sut.PurchaseDate, sut.PurchasePrice);
                var same = new Lot(sut.Id, "ANY", DateTime.Now.AddDays(-1), 100m);
                var notSame = new Lot(Guid.NewGuid(), sut.InstrumentSymbol, sut.PurchaseDate, sut.PurchasePrice);
                var different = new Lot(Guid.NewGuid(), "SYM2", DateTime.Now.AddDays(-10), 123m);
                var veryDifferent = new object();

                return new[]
                {
                    new object[] { sut, sut, true },
                    new object[] { sut, clone, true },
                    new object[] { sut, same, true },
                    new object[] { sut, notSame, false },
                    new object[] { sut, different, false },
                    new object[] { sut, null, false },
                    new object[] { sut, veryDifferent, false }
                };
            }
        }

        [Theory]
        [MemberData(nameof(IsSameAs_TestData))]
        public void IsSameAs_Compares_As_Entities(Lot sut, object other, bool same)
        {
            //act.
            var isSame = sut.IsSameAs(other);

            //assert.
            isSame.Should().Be(same);
        }
    }
}
