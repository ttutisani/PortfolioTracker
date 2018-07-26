using Gherkin.Ast;
using PortfolioTracker.Core;
using System;
using System.Linq;
using Xunit;
using Xunit.Gherkin.Quick;

namespace PortfolioTracker.AcceptanceTests
{
    [FeatureFile("./InstrumentPriceChange/InstrumentPriceChange.feature")]
    public sealed class InstrumentPriceChange : Xunit.Gherkin.Quick.Feature
    {
        private readonly InMemoryApp _app = new InMemoryApp();

        [Given(@"I have instrument with this info:")]
        public void Given_I_have_instrument_with_this_info(DataTable info)
        {
            var symbol = info.GetValue<string>(0, "Symbol");
            var name = info.GetValue<string>(0, "Name");
            var currentPrice = info.GetValue<decimal>(0, "Current Price");

            var instrument = new Instrument(symbol, name, currentPrice);
            _app.InstrumentRepository.Instruments.Add(instrument);
        }
        
        [And(@"I have entered lot with this info:")]
        public void And_I_have_entered_lot_with_this_info(DataTable info)
        {
            var purchaseDate = info.GetValue<DateTime>(0, "Purchase Date");
            var purchasePrice = info.GetValue<decimal>(0, "Purchase Price");
            var notes = info.GetValue<string>(0, "Notes");
            var instrumentSymbol = info.GetValue<string>(0, "Instrument Symbol");
            var instrumentName = info.GetValue<string>(0, "Instrument Name");
            var instrumentCurrentPrice = info.GetValue<decimal>(0, "Instrument Current Price");

            var lot = new Lot(Guid.NewGuid(),
                new InstrumentInfo(instrumentSymbol, instrumentName, instrumentCurrentPrice),
                purchaseDate,
                purchasePrice,
                notes);
            _app.LotRepository.Lots.Add(lot);

            _app.InstrumentRepository.Instruments.Single().AttachLotId(lot.Id);
        }

        [When(@"I change (\w{3}) instrument price to ([\d\.]+)")]
        public void When_I_change_instrument_price(string symbol, decimal newPrice)
        {
            _app.PortfolioTrackerApp.GetInstrumentService().UpdateInstrumentPrice(symbol, newPrice);
        }

        [Then(@"I should have instrument with this info:")]
        public void Then_I_should_have_instrument_with_this_info(DataTable info)
        {
            var symbol = info.GetValue<string>(0, "Symbol");
            var name = info.GetValue<string>(0, "Name");
            var currentPrice = info.GetValue<decimal>(0, "Current Price");

            Assert.Single(_app.InstrumentRepository.Instruments);
            var instrument = _app.InstrumentRepository.Instruments[0];

            Assert.NotNull(instrument);
            Assert.Equal(symbol, instrument.Symbol);
            Assert.Equal(name, instrument.Name);
            Assert.Equal(currentPrice, instrument.CurrentPrice);
        }

        [And(@"I should have lot with this info:")]
        public void And_I_should_have_lot_with_this_info(DataTable info)
        {
            var purchaseDate = info.GetValue<DateTime>(0, "Purchase Date");
            var purchasePrice = info.GetValue<decimal>(0, "Purchase Price");
            var notes = info.GetValue<string>(0, "Notes");
            var instrumentSymbol = info.GetValue<string>(0, "Instrument Symbol");
            var instrumentName = info.GetValue<string>(0, "Instrument Name");
            var instrumentCurrentPrice = info.GetValue<decimal>(0, "Instrument Current Price");

            Assert.Single(_app.LotRepository.Lots);
            var lot = _app.LotRepository.Lots[0];

            Assert.NotNull(lot);
            Assert.Equal(purchaseDate, lot.PurchaseDate);
            Assert.Equal(purchasePrice, lot.PurchasePrice);
            Assert.Equal(notes, lot.Notes);

            Assert.NotNull(lot.InstrumentInfo);
            Assert.Equal(instrumentSymbol, lot.InstrumentInfo.Symbol);
            Assert.Equal(instrumentName, lot.InstrumentInfo.Name);
            Assert.Equal(instrumentCurrentPrice, lot.InstrumentInfo.CurrentPrice);
        }
    }
}
