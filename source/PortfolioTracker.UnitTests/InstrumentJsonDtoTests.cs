using FluentAssertions;
using PortfolioTracker.Core;
using PortfolioTracker.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class InstrumentJsonDtoTests
    {
        [Fact]
        public void FromInstrument_Copies_All_Values()
        {
            //arrange.
            var instrument = new Instrument(
                "SMB", 
                "name 123", 
                123.45m,
                new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                });

            //act.
            var dto = InstrumentJsonDto.FromInstrument(instrument);

            //assert.
            dto.Should().NotBeNull();
            dto.Symbol.Should().Be(instrument.Symbol);
            dto.Name.Should().Be(instrument.Name);
            dto.CurrentPrice.Should().Be(instrument.CurrentPrice);
            dto.LotIdList.Should().Equal(instrument.LotIdList);
        }

        [Fact]
        public void ToInstrument_Copies_All_Values()
        {
            //arrange.
            var sut = new InstrumentJsonDto
            {
                Symbol = "ABC",
                Name = "abc name here",
                CurrentPrice = 45.123m,
                LotIdList = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            //act.
            var instrument = sut.ToInstrument();

            //assert.
            instrument.Should().NotBeNull();
            instrument.Symbol.Should().Be(sut.Symbol);
            instrument.Name.Should().Be(sut.Name);
            instrument.CurrentPrice.Should().Be(sut.CurrentPrice);
            instrument.LotIdList.Should().Equal(sut.LotIdList);
        }
    }
}
