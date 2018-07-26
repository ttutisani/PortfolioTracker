using System;

namespace PortfolioTracker.AppServices
{
    public sealed class UpdateLotInstrumentPriceCommand
    {
        public UpdateLotInstrumentPriceCommand(Guid lotId)
        {
            LotId = lotId;
        }

        public Guid LotId { get; }
    }
}
