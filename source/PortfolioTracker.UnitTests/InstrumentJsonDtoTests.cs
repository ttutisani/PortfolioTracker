using FluentAssertions;
using PortfolioTracker.Core;
using PortfolioTracker.Infrastructure;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class InstrumentJsonDtoTests
    {
        [Fact]
        public void FromInstrument_Copies_All_Values()
        {
            //arrange.
            var instrument = new Instrument("SMB", "name 123", 123.45m);

            //act.
            var dto = InstrumentJsonDto.FromInstrument(instrument);

            //assert.
            dto.Should().NotBeNull();
            dto.Symbol.Should().Be(instrument.Symbol);
            dto.Name.Should().Be(instrument.Name);
            dto.CurrentPrice.Should().Be(instrument.CurrentPrice);
        }

        [Fact]
        public void ToInstrument_Copies_All_Values()
        {
            //arrange.
            var sut = new InstrumentJsonDto
            {
                Symbol = "ABC",
                Name = "abc name here",
                CurrentPrice = 45.123m
            };

            //act.
            var instrument = sut.ToInstrument();

            //assert.
            instrument.Should().NotBeNull();
            instrument.Symbol.Should().Be(sut.Symbol);
            instrument.Name.Should().Be(sut.Name);
            instrument.CurrentPrice.Should().Be(sut.CurrentPrice);
        }
    }
}
