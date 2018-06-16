using PortfolioTracker.Core;
using System;

namespace PortfolioTracker.AppServices
{
    public sealed class LotService : ILotService
    {
        private readonly ILotRepository _lotRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IEventManagerSource _eventManagerSource;

        public LotService(
            ILotRepository lotRepository,
            IInstrumentRepository instrumentRepository,
            IEventManagerSource eventManagerSource)
        {
            _lotRepository = lotRepository ?? throw new ArgumentNullException(nameof(lotRepository));
            _instrumentRepository = instrumentRepository ?? throw new ArgumentNullException(nameof(instrumentRepository));
            _eventManagerSource = eventManagerSource ?? throw new ArgumentNullException(nameof(eventManagerSource));
        }

        public void AddLot(
            string symbol,
            DateTime purchaseDate,
            decimal purchasePrice,
            string notes = null
            )
        {
            using (var events = _eventManagerSource.Create())
            {
                var existingInstrument = _instrumentRepository.GetById(symbol);

                var newLot = existingInstrument != null
                    ? LotFactory.NewLotForExistingInstrument(existingInstrument, purchaseDate, purchasePrice, notes, events)
                    : LotFactory.NewLotForNewInstrument(symbol, purchaseDate, purchasePrice, notes, events);
                _lotRepository.Add(newLot);
            }
        }
    }
}
