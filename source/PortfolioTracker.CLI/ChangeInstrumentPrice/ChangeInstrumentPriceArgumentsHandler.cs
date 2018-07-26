using PortfolioTracker.CLI.ArgumentHandling;
using System;

namespace PortfolioTracker.CLI.ChangeInstrumentPrice
{
    public sealed class ChangeInstrumentPriceArgumentsHandler : IArgumentsHandler<ChangeInstrumentPriceArguments>
    {
        private readonly AppServices.IInstrumentService _instrumentService;

        public ChangeInstrumentPriceArgumentsHandler(AppServices.IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService ?? throw new System.ArgumentNullException(nameof(instrumentService));
        }

        public void Handle(ChangeInstrumentPriceArguments arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            _instrumentService.UpdateInstrumentPrice(arguments.Symbol, arguments.NewPrice);
        }
    }
}
