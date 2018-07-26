using PortfolioTracker.Core;
using System;

namespace PortfolioTracker.AppServices
{
    public sealed class ToUpdateLotInstrumentPrice : ICommandHandler<UpdateLotInstrumentPriceCommand>
    {
        private readonly ILotRepository _lotRepository;
        private readonly IInstrumentRepository _instrumentRepository;

        public ToUpdateLotInstrumentPrice(
            ILotRepository lotRepository,
            IInstrumentRepository instrumentRepository)
        {
            _lotRepository = lotRepository ?? throw new ArgumentNullException(nameof(lotRepository));
            _instrumentRepository = instrumentRepository ?? throw new ArgumentNullException(nameof(instrumentRepository));
        }

        public void Execute(UpdateLotInstrumentPriceCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var lot = _lotRepository.GetById(command.LotId);
            if (lot == null)
                throw new InvalidOperationException($"Cannot find lot with id `{command.LotId}`.");

            var instrument = _instrumentRepository.GetById(lot.InstrumentInfo.Symbol);
            if (instrument == null)
                throw new InvalidOperationException($"Cannot find instrument with symbol `{lot.InstrumentInfo.Symbol}`.");

            lot.UpdateInstrumentPrice(instrument.CurrentPrice);

            _lotRepository.Update(lot);
        }
    }
}
