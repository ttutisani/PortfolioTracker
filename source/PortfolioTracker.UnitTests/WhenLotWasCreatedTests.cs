using Moq;
using PortfolioTracker.AppServices;
using PortfolioTracker.Core;
using System;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class WhenLotWasCreatedTests
    {
        private readonly Mock<ILotRepository> _lotRepository = new Mock<ILotRepository>();
        private readonly Mock<IInstrumentRepository> _instrumentRepository = new Mock<IInstrumentRepository>();
        private readonly WhenLotWasCreated _sut;

        public WhenLotWasCreatedTests()
        {
            _sut = new WhenLotWasCreated(
                _lotRepository.Object, 
                _instrumentRepository.Object, 
                new Mock<IEventManagerSource>().Object);
        }

        [Fact]
        public void Saves_Instrument_For_Further_Tracking_If_Does_Not_Exist()
        {
            //arrange.
            var lotCreatedEvent = new LotWasCreatedDomainEvent(Guid.NewGuid());

            var createdLot = new Lot(
                lotCreatedEvent.LotId, 
                new InstrumentInfo("SYM3", "name123", 123.45m), 
                new DateTime(2001, 2, 3), 
                123.11m, 
                "notes 123");

            _lotRepository.Setup(lr => lr.GetById(lotCreatedEvent.LotId))
                .Returns(createdLot)
                .Verifiable();

            _instrumentRepository.Setup(ir => ir.GetById(createdLot.InstrumentInfo.Symbol))
                .Returns(() => null)
                .Verifiable();

            //act.
            _sut.When(lotCreatedEvent);

            //assert.
            _lotRepository.Verify();

            _instrumentRepository.Verify();

            Predicate<Instrument> isFromLotAndRefersToLot = instrument => 
                instrument != null
                && instrument.Symbol == createdLot.InstrumentInfo.Symbol
                && instrument.Name == createdLot.InstrumentInfo.Name
                && instrument.CurrentPrice == createdLot.InstrumentInfo.CurrentPrice
                && instrument.LotIdList.Contains(createdLot.Id);

            _instrumentRepository.Verify(ir => ir.Add(It.Is<Instrument>(instrument => isFromLotAndRefersToLot(instrument))), Times.Once);
        }

        [Fact]
        public void Attaches_Lot_Id_To_Instrument_If_Already_Exists()
        {
            //arrange.
            var lotCreatedEvent = new LotWasCreatedDomainEvent(Guid.NewGuid());

            var createdLot = new Lot(
                lotCreatedEvent.LotId,
                new InstrumentInfo("SYM3", "name123", 123.45m),
                new DateTime(2001, 2, 3),
                123.11m,
                "notes 123");

            _lotRepository.Setup(lr => lr.GetById(lotCreatedEvent.LotId))
                .Returns(createdLot)
                .Verifiable();

            var existingInstrument = new Instrument(
                createdLot.InstrumentInfo.Symbol, 
                "name other", 
                111.22m);

            _instrumentRepository.Setup(ir => ir.GetById(createdLot.InstrumentInfo.Symbol))
                .Returns(existingInstrument)
                .Verifiable();

            //act.
            _sut.When(lotCreatedEvent);

            //assert.
            _lotRepository.Verify();

            _instrumentRepository.Verify();

            _instrumentRepository.Verify(ir => ir.Add(It.IsAny<Instrument>()), Times.Never);

            Predicate<Instrument> refersToLot = instrument =>
                instrument == existingInstrument
                && instrument.LotIdList.Contains(createdLot.Id);

            _instrumentRepository.Verify(ir => ir.Update(It.Is<Instrument>(instrument => refersToLot(instrument))), Times.Once);
        }
    }
}
