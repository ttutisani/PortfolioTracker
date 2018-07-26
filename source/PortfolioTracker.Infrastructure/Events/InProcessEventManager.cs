using PortfolioTracker.Core;
using StructureMap;
using System.Collections.Generic;

namespace PortfolioTracker.Infrastructure
{
    public sealed class InProcessEventManager : IEventManager
    {
        private readonly List<object> _pendingEvents = new List<object>();
        private readonly Container _container;

        public InProcessEventManager(Container container)
        {
            _container = container ?? throw new System.ArgumentNullException(nameof(container));
        }

        public void Dispose()
        {
            foreach (var pendingEvent in _pendingEvents)
            {
                var eventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(pendingEvent.GetType());
                var eventHandlerInstance = _container.GetInstance(eventHandlerType);

                var handlerMethodName = nameof(IDomainEventHandler<object>.When);
                var handlerMethod = eventHandlerType.GetMethod(handlerMethodName);
                handlerMethod.Invoke(eventHandlerInstance, new [] { pendingEvent });
            }
        }

        public void Raise(object domainEvent)
        {
            _pendingEvents.Add(domainEvent);
        }
    }
}
