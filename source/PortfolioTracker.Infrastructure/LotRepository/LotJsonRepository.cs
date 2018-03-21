using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioTracker.Core;

namespace PortfolioTracker.Infrastructure
{
    public sealed class LotJsonRepository : ILotRepository
    {
        private const string _lotsJsonFileName = "Lots.json";

        public void Add(Lot aggregateRoot)
        {
            var lotDtos = DataFolder.DeserializeFileContent<List<LotJsonDto>>(_lotsJsonFileName) ?? new List<LotJsonDto>();
            lotDtos.Add(LotJsonDto.FromLot(aggregateRoot));
            DataFolder.SerializeContentInfoFile(_lotsJsonFileName, lotDtos);
        }

        public Lot GetById(Guid id)
        {
            var lotDtos = DataFolder.DeserializeFileContent<List<LotJsonDto>>(_lotsJsonFileName) ?? new List<LotJsonDto>();
            return lotDtos.FirstOrDefault(l => l.Id == id)?.ToLot();
        }

        public void Update(Lot aggregateRoot)
        {
            var lotDtos = DataFolder.DeserializeFileContent<List<LotJsonDto>>(_lotsJsonFileName) ?? new List<LotJsonDto>();
            var matchingDto = lotDtos.FirstOrDefault(l => l.Id == aggregateRoot.Id);
            if (matchingDto != null)
            {
                lotDtos[lotDtos.IndexOf(matchingDto)] = LotJsonDto.FromLot(aggregateRoot);
                DataFolder.SerializeContentInfoFile(_lotsJsonFileName, lotDtos);
            }
        }
    }
}
