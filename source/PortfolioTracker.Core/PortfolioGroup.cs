using PortfolioTracker.Core.Markers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortfolioTracker.Core
{
    public sealed class PortfolioGroup : IEntity, IAggregateRoot
    {
        public PortfolioGroup(Guid id, string name, IList<Portfolio> portfolios = null, string notes = null)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Portfolios = new ReadOnlyCollection<Portfolio>(portfolios ?? new List<Portfolio>());
            Notes = notes;
        }

        public Guid Id { get; }

        public string Name { get; }

        public ReadOnlyCollection<Portfolio> Portfolios { get; }

        public string Notes { get; }

        #region

        public bool IsSameAs(IEntity other)
        {
            return other is PortfolioGroup otherPortfolioGroup
                ? Id == otherPortfolioGroup.Id
                : false;
        }

        #endregion
    }
}
