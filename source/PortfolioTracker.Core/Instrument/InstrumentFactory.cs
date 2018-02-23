namespace PortfolioTracker.Core
{
    public static class InstrumentFactory
    {
        public static Instrument NewInstrumentByInfo(InstrumentInfo instrumentInfo)
        {
            var newInstrument = new Instrument(
                instrumentInfo.Symbol,
                instrumentInfo.Name,
                instrumentInfo.CurrentPrice);

            return newInstrument;
        }
    }
}
