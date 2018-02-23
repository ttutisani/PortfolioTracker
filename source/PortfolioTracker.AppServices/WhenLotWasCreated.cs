using PortfolioTracker.Core;

namespace PortfolioTracker.AppServices
{
    public sealed class WhenLotWasCreated : IDomainEventHandler<LotWasCreatedDomainEvent>
    {
        private readonly ILotRepository _lotRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IEventManagerSource _eventManagerSource;

        public WhenLotWasCreated(
            ILotRepository lotRepository,
            IInstrumentRepository instrumentRepository,
            IEventManagerSource eventManagerSource)
        {
            _lotRepository = lotRepository ?? throw new System.ArgumentNullException(nameof(lotRepository));
            _instrumentRepository = instrumentRepository ?? throw new System.ArgumentNullException(nameof(instrumentRepository));
            _eventManagerSource = eventManagerSource ?? throw new System.ArgumentNullException(nameof(eventManagerSource));
        }

        public void When(LotWasCreatedDomainEvent domainEvent)
        {
            using (var events = _eventManagerSource.Create())
            {
                var createdLot = _lotRepository.GetById(domainEvent.LotId);
                StartTrackingInstrumentForLot(createdLot);
            }
        }

        private void StartTrackingInstrumentForLot(Lot createdLot)
        {
            var existingInstrument = _instrumentRepository.GetById(createdLot.InstrumentInfo.Symbol);
            if (existingInstrument != null)
            {
                existingInstrument.AttachLotId(createdLot.Id);
                _instrumentRepository.Update(existingInstrument);
            }
            else
            {
                var newInstrument = InstrumentFactory.NewInstrumentByInfo(createdLot.InstrumentInfo);
                newInstrument.AttachLotId(createdLot.Id);
                _instrumentRepository.Add(newInstrument);
            }
        }
    }
}
