using Moq;
using PortfolioTracker.AppServices;
using PortfolioTracker.CLI.ChangeInstrumentPrice;
using System;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class ChangeInstrumentPriceArgumentsHandlerTests
    {
        private readonly Mock<IInstrumentService> _instrumentService = new Mock<IInstrumentService>();
        private readonly ChangeInstrumentPriceArgumentsHandler _sut;

        public ChangeInstrumentPriceArgumentsHandlerTests()
        {
            _sut = new ChangeInstrumentPriceArgumentsHandler(_instrumentService.Object);
        }

        [Fact]
        public void Handle_Requires_Arguments()
        {
            //act / assert.
            Assert.Throws<ArgumentNullException>(() => _sut.Handle(null));
        }

        [Fact]
        public void Handle_Changes_Price_Through_Service()
        {
            //arrange.
            var symbol = "CB1";
            var newPrice = 221.34m;

            //act.
            _sut.Handle(new ChangeInstrumentPriceArguments { Symbol = symbol, NewPrice = newPrice });

            //assert.
            _instrumentService.Verify(s => s.UpdateInstrumentPrice(symbol, newPrice), Times.Once);
        }
    }
}
