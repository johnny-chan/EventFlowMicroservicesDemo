using EventFlow.Aggregates;
using EventFlow.MsSql.ReadStores.Attributes;
using EventFlow.ReadStores;

namespace Common
{
    public class AnotherExampleReadModel : IReadModel, IAmReadModelFor<ExampleAggregate, ExampleId, AnotherExampleEvent>
    {
        [MsSqlReadModelIdentityColumn]
        public string AggregateId { get; set; }

        [MsSqlReadModelVersionColumn]
        public int Version { get; private set; }


        public int MagicNumber { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<ExampleAggregate, ExampleId, AnotherExampleEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            MagicNumber = domainEvent.AggregateEvent.MagicNumber * 2;
        }
    }
}
