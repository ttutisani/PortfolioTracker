﻿using FluentAssertions;
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
            var instrumentInfo = new InstrumentInfo("SYM", "name", 123.45m);
            var purchasePrice = 1m;
            var notes = "some notes";

            //act.
            var sut = new Lot(id, instrumentInfo, purchaseDate, purchasePrice, notes);

            //assert.
            sut.Id.Should().Be(id);
            sut.PurchaseDate.Should().Be(purchaseDate);
            sut.InstrumentInfo.Should().Be(instrumentInfo);
            sut.PurchasePrice.Should().Be(purchasePrice);
            sut.Notes.Should().Be(notes);
        }

        public static object[][] IsSameAs_TestData
        {
            get
            {
                var sut = new Lot(Guid.NewGuid(), new InstrumentInfo("SYM", "name", 123.45m), DateTime.Now, 10m);
                var clone = new Lot(sut.Id, sut.InstrumentInfo, sut.PurchaseDate, sut.PurchasePrice);
                var same = new Lot(sut.Id, new InstrumentInfo("ANY", "name", 123.45m), DateTime.Now.AddDays(-1), 100m);
                var notSame = new Lot(Guid.NewGuid(), sut.InstrumentInfo, sut.PurchaseDate, sut.PurchasePrice);
                var different = new Lot(Guid.NewGuid(), new InstrumentInfo("SYM2", "name", 123.45m), DateTime.Now.AddDays(-10), 123m);
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

        [Fact]
        public void UpdateInstrumentPrice_Replaces_Instrument_ValueObject()
        {
            //arrange.
            var instrumentInfo = new InstrumentInfo("GG1", "Golden Gates 1", 1.23m);
            var sut = new Lot(Guid.NewGuid(), instrumentInfo, new DateTime(2017, 12, 24), 1.23m);

            var newPrice = 2.34m;

            //act.
            sut.UpdateInstrumentPrice(newPrice);

            //assert.
            sut.InstrumentInfo.Should().NotBeNull();
            sut.InstrumentInfo.Should().NotBe(instrumentInfo);
            sut.InstrumentInfo.IsSameAs(instrumentInfo).Should().BeFalse();
            sut.InstrumentInfo.Symbol.Should().Be(instrumentInfo.Symbol);
            sut.InstrumentInfo.Name.Should().Be(instrumentInfo.Name);
            sut.InstrumentInfo.CurrentPrice.Should().Be(newPrice);
        }
    }
}
