using FluentAssertions;
using PortfolioTracker.Core;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class InstrumentInfoTests
    {
        [Fact]
        public void Ctor_Initializes_All_Properties()
        {
            //arrange.
            var symbol = "sym";
            var name = "name123";
            var currentPrice = 123.45m;

            //act.
            var sut = new InstrumentInfo(symbol, name, currentPrice);

            //assert.
            sut.Symbol.Should().Be(symbol);
            sut.Name.Should().Be(name);
            sut.CurrentPrice.Should().Be(currentPrice);
        }

        public static object[][] IsSameAs_TestData
        {
            get
            {
                var sut = new InstrumentInfo("sym", "name123", 123.45m);
                var same = new InstrumentInfo(sut.Symbol, sut.Name, sut.CurrentPrice);
                var different = new InstrumentInfo(sut.Symbol, sut.Name, sut.CurrentPrice + 0.01m);
                var veryDifferent = new object();

                return new []
                {
                    new object[] { sut, sut, true },
                    new object[] { sut, same, true },
                    new object[] { sut, different, false },
                    new object[] { sut, veryDifferent, false },
                    new object[] { sut, null, false }
                };
            }
        }

        [Theory]
        [MemberData(nameof(IsSameAs_TestData))]
        public void IsSameAs_Compares_As_ValueObjects(InstrumentInfo sut, object other, bool same)
        {
            //act.
            var isSame = sut.IsSameAs(other);

            //assert.
            isSame.Should().Be(same);
        }
    }
}
