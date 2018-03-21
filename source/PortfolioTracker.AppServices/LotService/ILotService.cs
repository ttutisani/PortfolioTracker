using System;

namespace PortfolioTracker.AppServices
{
    public interface ILotService
    {
        void AddLot(string symbol, DateTime purchaseDate, decimal purchasePrice, string notes = null);
    }
}