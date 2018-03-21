using PortfolioTracker.CLI.ArgumentHandling;

namespace PortfolioTracker.CLI.AddLot
{
    public sealed class AddLotArgumentsHandler : IArgumentsHandler<AddLotArguments>
    {
        private readonly AppServices.ILotService _lotService;

        public AddLotArgumentsHandler(AppServices.ILotService lotService)
        {
            _lotService = lotService ?? throw new System.ArgumentNullException(nameof(lotService));
        }

        public void Handle(AddLotArguments arguments)
        {
            if (arguments == null)
                throw new System.ArgumentNullException(nameof(arguments));

            _lotService.AddLot(
                arguments.Symbol, 
                arguments.PurchaseDate, 
                arguments.PurchasePrice, 
                arguments.Notes);
        }
    }
}
