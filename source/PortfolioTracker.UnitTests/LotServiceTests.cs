using Moq;
using PortfolioTracker.AppServices;
using PortfolioTracker.Core;
using System;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class LotServiceTests
    {
        private readonly Mock<ILotRepository> _lotRepository = new Mock<ILotRepository>();
        private readonly Mock<IEventManagerSource> _eventManagerSource = new Mock<IEventManagerSource>();
        private readonly Mock<IEventManager> _eventManager = new Mock<IEventManager>();
        private readonly LotService _sut;

        public LotServiceTests()
        {
            _eventManagerSource.Setup(ems => ems.Create()).Returns(_eventManager.Object);
            _sut = new LotService(_lotRepository.Object, _eventManagerSource.Object);
        }

        [Fact]
        public void AddLot_Creates_Lot_With_Instrument_Symbol()
        {
            //arrange.
            var symbol = "sym1";
            var purchaseDate = new DateTime(2000, 1, 2);
            var purchasePrice = 123.45m;
            var notes = "notes 123";

            //act.
            _sut.AddLot(symbol, purchaseDate, purchasePrice, notes);

            //assert.
            Lot createdLot = null;

            Predicate<Lot> hasAllInfo = lot => 
                (createdLot = lot) == lot
                && lot.Id != Guid.Empty
                && lot.InstrumentInfo != null
                && lot.InstrumentInfo.Symbol == symbol
                && lot.InstrumentInfo.CurrentPrice == purchasePrice
                && lot.PurchaseDate == purchaseDate
                && lot.PurchasePrice == purchasePrice;

            Predicate<object> isAboutLotCreation = evt =>
                evt is LotWasCreatedDomainEvent createdEvt
                && createdLot != null
                && createdEvt.LotId == createdLot.Id;

            _lotRepository.Verify(r => r.Add(It.Is<Lot>(lot => hasAllInfo(lot))), Times.Once);
            _eventManager.Verify(m => m.Raise(It.Is<object>(evt => isAboutLotCreation(evt))), Times.Once);
        }
    }
}
