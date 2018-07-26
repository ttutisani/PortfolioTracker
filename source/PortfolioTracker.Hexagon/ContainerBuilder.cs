using PortfolioTracker.AppServices;
using PortfolioTracker.Core;
using PortfolioTracker.Infrastructure;
using StructureMap;

namespace PortfolioTracker.Hexagon
{
    internal static class ContainerBuilder
    {
        public static Container BuildContainer()
        {
            var container = new Container();

            container.Configure(_ => 
            {
                //events.
                _.For<IEventManagerSource>().Use(context => new InProcessEventManagerSource(container));
                _.Scan(cfg => 
                {
                    cfg.AssemblyContainingType<IAssemblyMarker>();
                    cfg.ConnectImplementationsToTypesClosing(typeof(IDomainEventHandler<>));
                });

                //commands.
                _.For<ICommandManagerSource>().Use(context => new InProcessCommandManagerSource(container));
                _.Scan(cfg =>
                {
                    cfg.AssemblyContainingType<IAssemblyMarker>();
                    cfg.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                });

                _.For<IInstrumentRepository>().Use<InstrumentJsonRepository>();
                _.For<ILotRepository>().Use<LotJsonRepository>();
            });

            return container;
        }
    }
}
