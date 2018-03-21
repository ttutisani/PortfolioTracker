using PortfolioTracker.Core;
using System;

namespace PortfolioTracker.AppServices
{
    public sealed class LotService : ILotService
    {
        private readonly ILotRepository _lotRepository;
        private readonly IEventManagerSource _eventManagerSource;

        public LotService(
            ILotRepository lotRepository,
            IEventManagerSource eventManagerSource)
        {
            _lotRepository = lotRepository ?? throw new ArgumentNullException(nameof(lotRepository));
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
                var newLot = LotFactory.NewLotWithSymbol(symbol, purchaseDate, purchasePrice, notes, events);
                _lotRepository.Add(newLot);
            }
        }
    }
}
