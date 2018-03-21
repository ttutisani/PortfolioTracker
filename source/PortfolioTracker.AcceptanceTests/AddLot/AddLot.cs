using System;
using Xunit.Gherkin.Quick;
using Xunit;

namespace PortfolioTracker.AcceptanceTests
{
    [FeatureFile("./AddLot/AddLot.feature")]
    public sealed class AddLot : Feature
    {
        private readonly InMemoryApp _app = new InMemoryApp();

        [Given(@"there are no Lots yet in the system")]
        public void There_are_no_lots_in_the_system()
        {
            _app.LotRepository.Lots.Clear();
        }

        [And(@"there are no Instruments yet in the system")]
        public void There_are_no_instruments_yet_in_the_system()
        {
            _app.InstrumentRepository.Instruments.Clear();
        }

        [And(@"there is an existing Instrument with this info: symbol (\w{3}), name ""([\w\s]+)"", current price \$([\d\.]+)")]
        public void There_is_an_existing_instrument_with_this_info(string symbol, string name, decimal currentPrice)
        {
            _app.InstrumentRepository.Instruments.Clear();
            _app.InstrumentRepository.Instruments.Add(new Core.Instrument(symbol, name, currentPrice));
        }

        [When(@"I add Lot with this info: symbol (\w{3}), purchase date (\d{2}/\d{2}/\d{4}), purchase price \$([\d\.]+), notes ""([\w\s]+)""")]
        public void I_add_lot_with_this_info(string symbol, DateTime purchaseDate, decimal purchasePrice, string notes)
        {
            _app.PortfolioTrackerApp.GetLotService().AddLot(symbol, purchaseDate, purchasePrice, notes);
        }

        [Then(@"a new Lot must appear in the system with this info: symbol (\w{3}), purchase date (\d{2}/\d{2}/\d{4}), purchase price \$([\d\.]+), notes ""([\w\s]+)"", Instrument price \$([\d\.]+)")]
        public void A_new_lot_must_appear_in_the_system_with_this_info(string symbol, DateTime purchaseDate, decimal purchasePrice, string notes, decimal instrumentPrice)
        {
            Assert.Single(_app.LotRepository.Lots);

            var addedLot = _app.LotRepository.Lots[0];
            Assert.NotNull(addedLot);
            Assert.NotNull(addedLot.InstrumentInfo);
            Assert.Equal(symbol, addedLot.InstrumentInfo.Symbol);
            Assert.Equal(instrumentPrice, addedLot.InstrumentInfo.CurrentPrice);
            Assert.Equal(symbol, addedLot.InstrumentInfo.Name);
            Assert.Equal(purchaseDate, addedLot.PurchaseDate);
            Assert.Equal(purchasePrice, addedLot.PurchasePrice);
            Assert.Equal(notes, addedLot.Notes);
        }

        [And(@"a new Instrument must appear in the system with this info: symbol (\w{3}), name ""([\w\s]+)"", current price \$([\d\.]+)")]
        public void And_a_new_instrument_must_appear_in_the_system_with_this_info(string symbol, string name, decimal currentPrice)
        {
            Assert.Single(_app.InstrumentRepository.Instruments);

            var addedInstrument = _app.InstrumentRepository.Instruments[0];
            Assert.NotNull(addedInstrument);
            Assert.Equal(symbol, addedInstrument.Symbol);
            Assert.Equal(name, addedInstrument.Name);
            Assert.Equal(currentPrice, addedInstrument.CurrentPrice);
        }

        [And(@"an existing Instrument should stay with this info: symbol (\w{3}), name ""([\w\s]+)"", current price \$([\d\.]+)")]
        public void And_an_existing_instrument_should_stay_with_this_info(string symbol, string name, decimal currentPrice)
        {
            Assert.Single(_app.InstrumentRepository.Instruments);

            var addedInstrument = _app.InstrumentRepository.Instruments[0];
            Assert.NotNull(addedInstrument);
            Assert.Equal(symbol, addedInstrument.Symbol);
            Assert.Equal(name, addedInstrument.Name);
            Assert.Equal(currentPrice, addedInstrument.CurrentPrice);
        }
    }
}
