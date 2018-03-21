using PortfolioTracker.Core;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioTracker.Infrastructure
{
    public sealed class InstrumentJsonRepository : IInstrumentRepository
    {
        private const string _instrumentsJsonFileName = "Instruments.json";

        public void Add(Instrument aggregateRoot)
        {
            var instrumentDtos = DataFolder.DeserializeFileContent<List<InstrumentJsonDto>>(_instrumentsJsonFileName) ?? new List<InstrumentJsonDto>();
            instrumentDtos.Add(InstrumentJsonDto.FromInstrument(aggregateRoot));
            DataFolder.SerializeContentInfoFile(_instrumentsJsonFileName, instrumentDtos);
        }

        public Instrument GetById(string id)
        {
            var instrumentDtos = DataFolder.DeserializeFileContent<List<InstrumentJsonDto>>(_instrumentsJsonFileName) ?? new List<InstrumentJsonDto>();
            return instrumentDtos.FirstOrDefault(i => i.Symbol == id)?.ToInstrument();
        }

        public void Update(Instrument aggregateRoot)
        {
            var instrumentDtos = DataFolder.DeserializeFileContent<List<InstrumentJsonDto>>(_instrumentsJsonFileName) ?? new List<InstrumentJsonDto>();
            var matchingDto = instrumentDtos.FirstOrDefault(i => i.Symbol == aggregateRoot.Symbol);
            if (matchingDto != null)
            {
                instrumentDtos[instrumentDtos.IndexOf(matchingDto)] = InstrumentJsonDto.FromInstrument(aggregateRoot);
                DataFolder.SerializeContentInfoFile(_instrumentsJsonFileName, instrumentDtos);
            }
        }
    }
}
