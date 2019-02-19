using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace Common
{
    public class ExampleDuplicateReadModel : IReadModel, IAmReadModelFor<ExampleAggregate, ExampleId, ExampleMultiplerEvent>
    {
        public int MagicNumber { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<ExampleAggregate, ExampleId, ExampleMultiplerEvent> domainEvent)
        {
            MagicNumber = domainEvent.AggregateEvent.MagicNumber * 2;
        }
    }
}
