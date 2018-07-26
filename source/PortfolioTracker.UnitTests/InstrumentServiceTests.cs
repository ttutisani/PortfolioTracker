using Moq;
using PortfolioTracker.AppServices;
using PortfolioTracker.Core;
using Xunit;
using FluentAssertions;
using System;

namespace PortfolioTracker.UnitTests
{
    public sealed class InstrumentServiceTests
    {
        private readonly Mock<IInstrumentRepository> _instrumentRepository = new Mock<IInstrumentRepository>();
        private readonly Mock<IEventManagerSource> _eventManagerSource = new Mock<IEventManagerSource>();
        private readonly Mock<IEventManager> _eventManager = new Mock<IEventManager>();
        private readonly InstrumentService _sut;

        public InstrumentServiceTests()
        {
            _eventManagerSource.Setup(s => s.Create())
                .Returns(_eventManager.Object)
                .Verifiable();

            _sut = new InstrumentService(
                _instrumentRepository.Object,
                _eventManagerSource.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UpdateInstrumentPrice_Requires_Symbol(string symbol)
        {
            //act / assert.
            new Action(() => _sut.UpdateInstrumentPrice(symbol, 1.23m))
                .ShouldThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void UpdateInstrumentPrice_Requires_Positive_Price(
            decimal price)
        {
            //act / assert.
            new Action(() => _sut.UpdateInstrumentPrice("G12", price))
                .ShouldThrowExactly<ArgumentException>();
        }

        [Fact]
        public void UpdateInstrumentPrice_Throws_When_No_Such_Instrument()
        {
            //arrange.
            var symbol = "G12";
            _instrumentRepository.Setup(r => r.GetById(symbol))
                .Returns(() => null)
                .Verifiable();

            //act / assert.
            new Action(() => _sut.UpdateInstrumentPrice(symbol, 1.23m))
                .ShouldThrowExactly<InvalidOperationException>();

            _instrumentRepository.Verify();
            _eventManagerSource.Verify();
        }

        [Fact]
        public void UpdateInstrumentPrice_Changes_Instrument_Price()
        {
            //arrange.
            var symbol = "S12";
            var instrument = new Instrument(symbol, "some name", 1.23m);
            _instrumentRepository.Setup(r => r.GetById(symbol))
                .Returns(instrument)
                .Verifiable();

            var newPrice = 2.34m;

            //act.
            _sut.UpdateInstrumentPrice(symbol, newPrice);

            //assert.
            _instrumentRepository.Verify();

            _eventManagerSource.Verify();

            Predicate<object> isForPriceChange = obj =>
                obj is InstrumentPriceChangedDomainEvent priceChangedEvent
                && priceChangedEvent.InstrumentSymbol == symbol;
            _eventManager.Verify(m => m.Raise(It.Is<object>(o => isForPriceChange(o))), Times.Once);

            _instrumentRepository.Verify(
                r => r.Update(It.Is<Instrument>(i => i.CurrentPrice == newPrice)),
                Times.Once);

            instrument.CurrentPrice.Should().Be(newPrice);
        }
    }
}
