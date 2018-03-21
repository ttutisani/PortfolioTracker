using PortfolioTracker.Core;
using PortfolioTracker.Infrastructure;
using StructureMap;

namespace PortfolioTracker.Hexagon
{
    public static class ContainerBuilder
    {
        public static Container BuildContainer()
        {
            var container = new Container();

            container.Configure(_ => 
            {
                _.For<IEventManagerSource>().Use(context => new InProcessEventManagerSource(container));

                _.Scan(cfg => 
                {
                    //find all event handlers.
                    cfg.AssemblyContainingType<AppServices.IAssemblyMarker>();
                    cfg.ConnectImplementationsToTypesClosing(typeof(IDomainEventHandler<>));
                });

                _.For<IInstrumentRepository>().Use<InstrumentJsonRepository>();
                _.For<ILotRepository>().Use<LotJsonRepository>();
            });

            return container;
        }
    }
}
