using EventFlow.Aggregates;

namespace Common
{
    /// <summary>
    /// This event is for performing further calculation to the magic number
    /// </summary>
    public class ExampleMultiplerEvent : AggregateEvent<ExampleAggregate, ExampleId>
    {
        public ExampleMultiplerEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}
