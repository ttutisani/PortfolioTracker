using FluentAssertions;
using PortfolioTracker.Core;
using PortfolioTracker.Infrastructure;
using System;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class LotJsonDtoTests
    {
        [Fact]
        public void FromLot_Copies_All_Values()
        {
            //arrange.
            var lot = new Lot(
                Guid.NewGuid(),
                new InstrumentInfo("ABC", "name for abc", 123.45m),
                new DateTime(2008, 7, 6),
                120.21m,
                "some notes");

            //act.
            var dto = LotJsonDto.FromLot(lot);

            //assert.
            dto.Should().NotBeNull();
            dto.Id.Should().Be(lot.Id);

            dto.InstrumentInfo.Should().NotBeNull();
            dto.InstrumentInfo.Symbol.Should().Be(lot.InstrumentInfo.Symbol);
            dto.InstrumentInfo.Name.Should().Be(lot.InstrumentInfo.Name);
            dto.InstrumentInfo.CurrentPrice.Should().Be(lot.InstrumentInfo.CurrentPrice);

            dto.PurchaseDate.Should().Be(lot.PurchaseDate);
            dto.PurchasePrice.Should().Be(lot.PurchasePrice);
            dto.Notes.Should().Be(lot.Notes);
        }

        [Fact]
        public void ToLot_Copies_All_Values()
        {
            //arrange.
            var sut = new LotJsonDto
            {
                Id = Guid.NewGuid(),

                InstrumentInfo = new InstrumentInfoJsonDto
                {
                    Symbol = "BCA",
                    Name = "BCA name 123",
                    CurrentPrice = 321.54m
                },

                PurchaseDate = new DateTime(2011, 12, 13),
                PurchasePrice = 123.45m,
                Notes = "Notes asd123"
            };

            //act.
            var lot = sut.ToLot();

            //assert.
            lot.Should().NotBeNull();
            lot.Id.Should().Be(sut.Id);

            lot.InstrumentInfo.Should().NotBeNull();
            lot.InstrumentInfo.Symbol.Should().Be(sut.InstrumentInfo.Symbol);
            lot.InstrumentInfo.Name.Should().Be(sut.InstrumentInfo.Name);
            lot.InstrumentInfo.CurrentPrice.Should().Be(sut.InstrumentInfo.CurrentPrice);

            lot.PurchaseDate.Should().Be(sut.PurchaseDate);
            lot.PurchasePrice.Should().Be(sut.PurchasePrice);
            lot.Notes.Should().Be(sut.Notes);
        }
    }
}
