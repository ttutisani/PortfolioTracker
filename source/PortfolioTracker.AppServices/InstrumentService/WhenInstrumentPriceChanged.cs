using PortfolioTracker.Core;
using System;

namespace PortfolioTracker.AppServices
{
    public sealed class WhenInstrumentPriceChanged : IDomainEventHandler<InstrumentPriceChangedDomainEvent>
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ICommandManagerSource _commandManagerSource;

        public WhenInstrumentPriceChanged(
            IInstrumentRepository instrumentRepository,
            ICommandManagerSource commandManagerSource)
        {
            _instrumentRepository = instrumentRepository ?? throw new System.ArgumentNullException(nameof(instrumentRepository));
            _commandManagerSource = commandManagerSource ?? throw new System.ArgumentNullException(nameof(commandManagerSource));
        }

        public void When(InstrumentPriceChangedDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            using (var commands = _commandManagerSource.Create())
            {
                var instrument = _instrumentRepository.GetById(domainEvent.InstrumentSymbol);
                if (instrument == null)
                    throw new InvalidOperationException($"Cannot find instrument with symbol `{domainEvent.InstrumentSymbol}`.");

                foreach (var lotId in instrument.LotIdList)
                {
                    commands.Send(new UpdateLotInstrumentPriceCommand(lotId));
                }
            }
        }
    }
}
