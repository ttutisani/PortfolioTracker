using FluentAssertions;
using Moq;
using PortfolioTracker.Core;
using PortfolioTracker.Core.Markers;
using System;
using Xunit;

namespace PortfolioTracker.UnitTests
{
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
                var same = new Instrument("SYM", "other name", 100m);
                var different = new Instrument("MYS", "name", 10m);
                var otherEntity = new Mock<IEntity>().Object;
                var otherObject = new Object();

                return new[]
                {
                new object[] { sut, sut, true },
                new object[] { sut, same, true },
                new object[] { sut, different, false },
                new object[] { sut, null, false },
                new object[] { sut, otherEntity, false },
                new object[] { sut, otherObject, false }
            };
            }
        }

        [Theory]
        [MemberData(nameof(IsSameAs_Test_Data))]
        public void IsSameAs_Compares_As_Entities(Instrument sut, object other, bool same)
        {
            //act.
            var isSame = sut.IsSameAs(other);

            //assert.
            isSame.Should().Be(same);
        }

    }
}
