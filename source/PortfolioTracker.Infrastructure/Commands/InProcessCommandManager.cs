using PortfolioTracker.AppServices;
using StructureMap;
using System.Collections.Generic;

namespace PortfolioTracker.Infrastructure
{
    public sealed class InProcessCommandManager : ICommandManager
    {
        private readonly List<object> _pendingCommands = new List<object>();
        private readonly Container _container;

        public InProcessCommandManager(Container container)
        {
            _container = container ?? throw new System.ArgumentNullException(nameof(container));
        }

        public void Dispose()
        {
            foreach (var pendingCommand in _pendingCommands)
            {
                var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(pendingCommand.GetType());
                var commandHandlerInstance = _container.GetInstance(commandHandlerType);

                var handlerMethodName = nameof(ICommandHandler<object>.Execute);
                var handlerMethod = commandHandlerType.GetMethod(handlerMethodName);
                handlerMethod.Invoke(commandHandlerInstance, new[] { pendingCommand });
            }
        }

        public void Send(object command)
        {
            _pendingCommands.Add(command);
        }
    }
}
