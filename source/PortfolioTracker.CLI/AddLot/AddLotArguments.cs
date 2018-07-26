using CommandLine;
using System;

namespace PortfolioTracker.CLI.AddLot
{
    /// <summary>
    /// Command line arguments for adding lot.
    /// </summary>
    /// <example>
    /// $ add-lot ABC 01/02/2003 123.45 --notes="my notes 123"
    /// </example>
    [Verb("add-lot")]
    public sealed class AddLotArguments
    {
        [Value(0, Required = true)]
        public string Symbol { get; set; }

        [Value(1, Required = true)]
        public DateTime PurchaseDate { get; set; }

        [Value(2, Required = true)]
        public decimal PurchasePrice { get; set; }

        [Option]
        public string Notes { get; set; }
    }
}
