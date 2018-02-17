using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortfolioTracker.Core
{
    public sealed class Portfolio : Markers.IAggregateRoot
    {
        public Portfolio(Guid id, string name, IList<Holding> holdings = null, string notes = null)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Holdings = new ReadOnlyCollection<Holding>(holdings ?? new List<Holding>());
            Notes = notes;
        }

        public Guid Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<Holding> Holdings { get; }

        public string Notes { get; }

        public bool IsSameAs(object other)
        {
            return other is Portfolio otherPortfolio
                ? Id == otherPortfolio.Id
                : false;
        }
    }
}
