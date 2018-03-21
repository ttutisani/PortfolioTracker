using PortfolioTracker.Core;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioTracker.AcceptanceTests.Stubs
{
    public sealed class InMemoryInstrumentRepository : IInstrumentRepository
    {
        public List<Instrument> Instruments { get; } = new List<Instrument>();

        public void Add(Instrument aggregateRoot)
        {
            Instruments.Add(aggregateRoot);
        }

        public Instrument GetById(string id)
        {
            return Instruments.FirstOrDefault(i => i.Symbol == id);
        }

        public void Update(Instrument aggregateRoot)
        {
            var existingInstrument = Instruments.FirstOrDefault(i => i.IsSameAs(aggregateRoot));
            if (existingInstrument != null)
                Instruments[Instruments.IndexOf(existingInstrument)] = aggregateRoot;
        }
    }
}
