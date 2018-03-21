using System;
using PortfolioTracker.Core;

namespace PortfolioTracker.Infrastructure
{
    internal sealed class LotJsonDto
    {
        public Guid Id { get; set; }
        public InstrumentInfoJsonDto InstrumentInfo { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Notes { get; set; }

        public static LotJsonDto FromLot(Lot lot)
        {
            return new LotJsonDto
            {
                Id = lot.Id,
                InstrumentInfo = InstrumentInfoJsonDto.FromInstrumentInfo(lot.InstrumentInfo),
                PurchaseDate = lot.PurchaseDate,
                PurchasePrice = lot.PurchasePrice,
                Notes = lot.Notes
            };
        }

        public Lot ToLot()
        {
            return new Lot(
                Id,
                InstrumentInfo.ToInstrumentInfo(),
                PurchaseDate,
                PurchasePrice,
                Notes);
        }
    }

    internal sealed class InstrumentInfoJsonDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }

        public static InstrumentInfoJsonDto FromInstrumentInfo(InstrumentInfo instrumentInfo)
        {
            return new InstrumentInfoJsonDto
            {
                Symbol = instrumentInfo.Symbol,
                Name = instrumentInfo.Name,
                CurrentPrice = instrumentInfo.CurrentPrice
            };
        }

        public InstrumentInfo ToInstrumentInfo()
        {
            return new InstrumentInfo(Symbol, Name, CurrentPrice);
        }
    }
}
