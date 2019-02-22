using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using EventFlow.Aggregates;
using EventFlow.Configuration;
using EventFlow.EventStores;
using EventFlow.Queries;
using EventFlow.Subscribers;
using Microsoft.AspNetCore.Mvc;

namespace ExampleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamplesMultiplerController : ControllerBase
    {
        private readonly IDomainEventPublisher _domainEventPublisher;
        private readonly IDomainEventFactory _domainEventFactory;
        private readonly IQueryProcessor _queryProcessor;


        public ExamplesMultiplerController(
            IResolver resolver,
            IQueryProcessor queryProcessor)
        {
            _domainEventPublisher = resolver.Resolve<IDomainEventPublisher>();
            _domainEventFactory = resolver.Resolve<IDomainEventFactory>();
            _queryProcessor = queryProcessor;
        }

        // GET api/examplesmultiplier/a6e02d4d-871e-4d18-be8a-b647706a2a11
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<AnotherExampleReadModel>> GetExample(string id)
        {
            var readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<AnotherExampleReadModel>(id), CancellationToken.None);

            return Ok(readModel);
        }

        // POST api/examplesMultipler
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> Post([FromBody] int value)
        {
            var exampleId = ExampleId.New;
            var multiplierEvent =
                _domainEventFactory.Create(new AnotherExampleEvent(value), new Metadata
                {
                    AggregateId = exampleId.Value,
                    Timestamp = DateTime.UtcNow
                }, exampleId.Value, 1);

            // The publishAsync api call does not persist the event to the EventFlow table.
            await _domainEventPublisher.PublishAsync(new List<IDomainEvent> { multiplierEvent }, CancellationToken.None);

            return CreatedAtAction(nameof(GetExample), new { id = exampleId.Value }, multiplierEvent);
        }
    }
}
