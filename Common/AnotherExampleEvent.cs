using EventFlow.Aggregates;

namespace Common
{
    /// <summary>
    /// This event is for performing further calculation to the magic number
    /// </summary>
    public class AnotherExampleEvent : AggregateEvent<ExampleAggregate, ExampleId>
    {
        public AnotherExampleEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}
