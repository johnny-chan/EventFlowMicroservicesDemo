using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace Common
{
    public class ExampleAggregate : AggregateRoot<ExampleAggregate, ExampleId>, IEmit<ExampleEvent>, IEmit<AnotherExampleEvent>
    {
       
        private int? _magicNumber;

        public ExampleAggregate(ExampleId id) : base(id) { }

        // Method invoked by our command
        public IExecutionResult SetMagicNumer(int magicNumber)
        {
            if (_magicNumber.HasValue)
                return ExecutionResult.Failed("Magic number already set");

            Emit(new ExampleEvent(magicNumber));

            return ExecutionResult.Success();
        }

        // We apply the event as part of the event sourcing system. EventFlow
        // provides several different methods for doing this, e.g. state objects,
        // the Apply method is merely the simplest
        public void Apply(ExampleEvent aggregateEvent)
        {
            _magicNumber = aggregateEvent.MagicNumber;
        }

        public void Apply(AnotherExampleEvent aggregateEvent)
        {
            _magicNumber = aggregateEvent.MagicNumber;
        }
    }
}
