using Moq;
using Xunit;

namespace PortfolioTracker.UnitTests
{
    public sealed class AddLotArgumentsHandlerTests
    {
        private readonly CLI.AddLot.AddLotArgumentsHandler _sut;
        private readonly Mock<AppServices.ILotService> _lotService = new Mock<AppServices.ILotService>();

        public AddLotArgumentsHandlerTests()
        {
            _sut = new CLI.AddLot.AddLotArgumentsHandler(_lotService.Object);
        }

        [Fact]
        public void Handle_Requires_Args()
        {
            Assert.Throws<System.ArgumentNullException>(() => _sut.Handle(null));
        }

        [Fact]
        public void Handle_Calls_LotService_With_All_Arguments()
        {
            //arrange.
            var addLotArgs = new CLI.AddLot.AddLotArguments
            {
                Symbol = "s123",
                PurchaseDate = new System.DateTime(2018, 2, 3),
                PurchasePrice = 1122.33m,
                Notes = "some notes 123"
            };

            //act.
            _sut.Handle(addLotArgs);

            //assert.
            _lotService.Verify(ls => ls.AddLot(
                addLotArgs.Symbol, 
                addLotArgs.PurchaseDate, 
                addLotArgs.PurchasePrice, 
                addLotArgs.Notes), Times.Once);
        }
    }
}
