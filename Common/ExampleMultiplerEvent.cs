using EventFlow.Aggregates;

namespace Common
{
    public class ExampleMultiplerEvent : AggregateEvent<ExampleAggregate, ExampleId>
    {
        public ExampleMultiplerEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}
