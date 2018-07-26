using PortfolioTracker.AppServices;
using Xunit;
using FluentAssertions;
using System;
using Moq;
using PortfolioTracker.Core;

namespace PortfolioTracker.UnitTests
{
    public sealed class ToUpdateLotInstrumentPriceTests
    {
        private readonly Mock<ILotRepository> _lotRepository = new Mock<ILotRepository>();
        private readonly Mock<IInstrumentRepository> _instrumentRepository = new Mock<IInstrumentRepository>();
        private readonly ToUpdateLotInstrumentPrice _sut;

        public ToUpdateLotInstrumentPriceTests()
        {
            _sut = new ToUpdateLotInstrumentPrice(
                _lotRepository.Object,
                _instrumentRepository.Object);
        }

        [Fact]
        public void Execute_Requires_Command()
        {
            //act / assert.
            new Action(() => _sut.Execute(null)).ShouldThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Execute_Throws_If_No_Such_Lot_Found()
        {
            //arrange.
            var updateLotCommand = new UpdateLotInstrumentPriceCommand(Guid.NewGuid());

            _lotRepository.Setup(r => r.GetById(updateLotCommand.LotId))
                .Returns(() => null)
                .Verifiable();

            //act / assert.
            new Action(() => _sut.Execute(updateLotCommand)).ShouldThrowExactly<InvalidOperationException>();

            _lotRepository.Verify();
        }

        [Fact]
        public void Execute_Throws_If_No_Such_Instrument_Found()
        {
            //arrange.
            var updateLotCommand = new UpdateLotInstrumentPriceCommand(Guid.NewGuid());

            var lot = new Lot(Guid.NewGuid(), new InstrumentInfo("GL1", "Goldish", 1.23m), new DateTime(2017, 12, 24), 1.23m);
            _lotRepository.Setup(r => r.GetById(updateLotCommand.LotId))
                .Returns(lot)
                .Verifiable();

            _instrumentRepository.Setup(r => r.GetById(lot.InstrumentInfo.Symbol))
                .Returns(() => null)
                .Verifiable();

            //act / assert.
            new Action(() => _sut.Execute(updateLotCommand)).ShouldThrowExactly<InvalidOperationException>();

            _lotRepository.Verify();
            _instrumentRepository.Verify();
        }

        [Fact]
        public void Execute_Updates_Lot_Instrument_Price()
        {
            //arrange.
            var updateLotCommand = new UpdateLotInstrumentPriceCommand(Guid.NewGuid());

            var lot = new Lot(Guid.NewGuid(), new InstrumentInfo("GL1", "Goldish", 1.23m), new DateTime(2017, 12, 24), 1.23m);
            _lotRepository.Setup(r => r.GetById(updateLotCommand.LotId))
                .Returns(lot)
                .Verifiable();

            var instrument = new Instrument(lot.InstrumentInfo.Symbol, lot.InstrumentInfo.Name, 2.34m);
            _instrumentRepository.Setup(r => r.GetById(lot.InstrumentInfo.Symbol))
                .Returns(instrument)
                .Verifiable();

            //act.
            _sut.Execute(updateLotCommand);

            //assert.
            _lotRepository.Verify();
            _instrumentRepository.Verify();

            Predicate<Lot> isUpdated = l => l == lot
                && l.InstrumentInfo.CurrentPrice == instrument.CurrentPrice;

            _lotRepository.Verify(r => r.Update(It.Is<Lot>(l => isUpdated(l))), Times.Once);
        }
    }
}
