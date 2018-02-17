using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PortfolioTracker.Core
{
    public sealed class Holding : Markers.IEntity
    {
        public Holding(Guid id, string instrumentSymbol, IList<Guid> lotIdList = null, string notes = null)
        {
            Id = id;
            InstrumentSymbol = instrumentSymbol ?? throw new ArgumentNullException(nameof(instrumentSymbol));

            ValidateLots(lotIdList);
            LotIdList = new ReadOnlyCollection<Guid>(lotIdList ?? new List<Guid>());

            Notes = notes;
        }

        private static void ValidateLots(IList<Guid> lotIdList)
        {
            if (lotIdList == null)
                return;

            if (lotIdList.Any(lotId => lotIdList.Count(otherLotId => otherLotId == lotId) > 1))
                throw new InvalidOperationException("Lots cannot repeat.");
        }

        public Guid Id { get; }

        public string InstrumentSymbol { get; }

        public ReadOnlyCollection<Guid> LotIdList { get; }

        public string Notes { get; }

        public bool IsSameAs(object other)
        {
            return other is Holding otherHolding
                ? Id == otherHolding.Id
                : false;
        }
    }
}
