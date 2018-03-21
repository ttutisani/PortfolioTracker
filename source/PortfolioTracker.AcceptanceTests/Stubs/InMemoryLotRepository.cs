using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioTracker.Core;

namespace PortfolioTracker.AcceptanceTests.Stubs
{
    public sealed class InMemoryLotRepository : ILotRepository
    {
        public List<Lot> Lots { get; } = new List<Lot>();

        public void Add(Lot aggregateRoot)
        {
            Lots.Add(aggregateRoot);
        }

        public Lot GetById(Guid id)
        {
            return Lots.FirstOrDefault(l => l.Id == id);
        }

        public void Update(Lot aggregateRoot)
        {
            var existingLot = Lots.FirstOrDefault(l => l.IsSameAs(aggregateRoot));
            if (existingLot != null)
                Lots[Lots.IndexOf(existingLot)] = aggregateRoot;
        }
    }
}
