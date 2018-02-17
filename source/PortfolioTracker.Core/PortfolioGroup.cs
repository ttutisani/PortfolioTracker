using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortfolioTracker.Core
{
    public sealed class PortfolioGroup : Markers.IAggregateRoot
    {
        public PortfolioGroup(
            Guid id, 
            string name, 
            IList<Guid> portfolioIdList = null, 
            string notes = null)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PortfolioIdList = new ReadOnlyCollection<Guid>(portfolioIdList ?? new List<Guid>());
            Notes = notes;
        }

        public Guid Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<Guid> PortfolioIdList { get; }

        public string Notes { get; }

        public bool IsSameAs(object other)
        {
            return other is PortfolioGroup otherPortfolioGroup
                ? Id == otherPortfolioGroup.Id
                : false;
        }
    }
}
