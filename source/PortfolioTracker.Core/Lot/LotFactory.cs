using System;

namespace PortfolioTracker.Core
{
    public static class LotFactory
    {
        public static Lot NewLotForNewInstrument(
            string symbol, 
            DateTime purchaseDate, 
            decimal purchasePrice, 
            string notes, 
            IEventManager events)
        {
            var instrumentInfoWithSymbolOnly = new InstrumentInfo(
                symbol,
                symbol,
                purchasePrice);

            var lot = new Lot(
                Guid.NewGuid(),
                instrumentInfoWithSymbolOnly,
                purchaseDate,
                purchasePrice,
                notes);

            events.Raise(new LotWasCreatedDomainEvent(lot.Id));

            return lot;
        }

        public static Lot NewLotForExistingInstrument(Instrument existingInstrument, DateTime purchaseDate, decimal purchasePrice, string notes, IEventManager events)
        {
            var instrumentInfoForExistingInstrument = new InstrumentInfo(
                existingInstrument.Symbol,
                existingInstrument.Name,
                existingInstrument.CurrentPrice);

            var lot = new Lot(
                Guid.NewGuid(),
                instrumentInfoForExistingInstrument,
                purchaseDate,
                purchasePrice,
                notes);

            events.Raise(new LotWasCreatedDomainEvent(lot.Id));

            return lot;
        }
    }
}
