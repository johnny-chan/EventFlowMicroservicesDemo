using EventFlow.Core;

namespace Common
{
    public class ExampleId : Identity<ExampleId>
    {
        public ExampleId(string value) : base(value) { }
    }
}
