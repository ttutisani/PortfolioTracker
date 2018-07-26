using PortfolioTracker.AppServices;
using Xunit;
using FluentAssertions;
using PortfolioTracker.Core;
using Moq;
using System;
using System.Collections.Generic;

namespace PortfolioTracker.UnitTests
{
    public sealed class WhenInstrumentPriceChangedTests
    {
        private readonly Mock<IInstrumentRepository> _instrumentRepository = new Mock<IInstrumentRepository>();
        private readonly Mock<ICommandManagerSource> _commandManagerSource = new Mock<ICommandManagerSource>();
        private readonly Mock<ICommandManager> _commandManager = new Mock<ICommandManager>();
        private readonly WhenInstrumentPriceChanged _sut;

        public WhenInstrumentPriceChangedTests()
        {
            _sut = new WhenInstrumentPriceChanged(
                _instrumentRepository.Object,
                _commandManagerSource.Object);

            _commandManagerSource.Setup(s => s.Create())
                .Returns(_commandManager.Object)
                .Verifiable();
        }

        [Fact]
        public void When_Requires_Event()
        {
            //act / assert.
            new Action(() => _sut.When(null)).ShouldThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void When_Throws_When_No_Such_Instrument()
        {
            //arrange.
            var priceChangedEvent = new InstrumentPriceChangedDomainEvent("GL2");

            _instrumentRepository.Setup(r => r.GetById(priceChangedEvent.InstrumentSymbol))
                .Returns(() => null)
                .Verifiable();

            //act / assert.
            new Action(() => _sut.When(priceChangedEvent)).ShouldThrowExactly<InvalidOperationException>();
            _commandManagerSource.Verify();
            _instrumentRepository.Verify();
        }

        [Fact]
        public void When_Sends_Instrument_Price_Update_Command_For_Lots()
        {
            //arrange.
            var priceChangedEvent = new InstrumentPriceChangedDomainEvent("GL2");

            var lotIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _instrumentRepository.Setup(r => r.GetById(priceChangedEvent.InstrumentSymbol))
                .Returns(new Instrument(priceChangedEvent.InstrumentSymbol, "Golden name", 2.34m, lotIds))
                .Verifiable();

            //act.
            _sut.When(priceChangedEvent);

            //assert.
            _commandManagerSource.Verify();

            _instrumentRepository.Verify();

            Func<object, int, bool> isForLotId = (o, index) => o is UpdateLotInstrumentPriceCommand updateLot
                && updateLot.LotId == lotIds[index];

            _commandManager.Verify(m => m.Send(It.IsAny<object>()), Times.Exactly(3));
            _commandManager.Verify(m => m.Send(It.Is<object>(o => isForLotId(o, 0))));
            _commandManager.Verify(m => m.Send(It.Is<object>(o => isForLotId(o, 1))));
            _commandManager.Verify(m => m.Send(It.Is<object>(o => isForLotId(o, 2))));
        }
    }
}
