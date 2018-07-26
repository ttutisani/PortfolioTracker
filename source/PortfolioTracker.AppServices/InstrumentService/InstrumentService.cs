using PortfolioTracker.Core;
using System;

namespace PortfolioTracker.AppServices
{
    public sealed class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IEventManagerSource _eventManagerSource;

        public InstrumentService(
            IInstrumentRepository instrumentRepository,
            IEventManagerSource eventManagerSource)
        {
            _instrumentRepository = instrumentRepository ?? throw new ArgumentNullException(nameof(instrumentRepository));
            _eventManagerSource = eventManagerSource ?? throw new ArgumentNullException(nameof(eventManagerSource));
        }

        public void UpdateInstrumentPrice(string symbol, decimal newPrice)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentNullException(nameof(symbol));

            if (newPrice <= 0)
                throw new ArgumentException($"`{nameof(newPrice)}` must be positive. Was `{newPrice}`.", nameof(newPrice));

            using (var events = _eventManagerSource.Create())
            {
                var instrument = _instrumentRepository.GetById(symbol);
                if (instrument == null)
                    throw new InvalidOperationException($"Instrumemt `{symbol}` not found.");

                instrument.UpdatePrice(newPrice, events);

                _instrumentRepository.Update(instrument);
            }
        }
    }
}
