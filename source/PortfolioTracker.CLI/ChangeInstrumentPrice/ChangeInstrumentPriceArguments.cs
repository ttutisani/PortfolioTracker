using CommandLine;

namespace PortfolioTracker.CLI.ChangeInstrumentPrice
{
    /// <summary>
    /// Command line arguments for changing instrument price.
    /// </summary>
    /// <example>
    /// $ set-instrument-price ABC 123.45
    /// </example>
    [Verb("set-instrument-price")]
    public sealed class ChangeInstrumentPriceArguments
    {
        [Value(0, Required = true)]
        public string Symbol { get; set; }

        [Value(1, Required = true)]
        public decimal NewPrice { get; set; }
    }
}
