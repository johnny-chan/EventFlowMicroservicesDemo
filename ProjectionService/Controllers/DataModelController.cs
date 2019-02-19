using System.Threading;
using System.Threading.Tasks;
using Common;
using EventFlow.Configuration;
using EventFlow.Queries;
using EventFlow.ReadStores;
using Microsoft.AspNetCore.Mvc;

namespace ProjectionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataModelController : ControllerBase
    {
        private readonly IReadModelPopulator _readModelPopulator;
        private readonly IQueryProcessor _queryProcessor;

        public DataModelController(
            IResolver resolver,
            IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
            _readModelPopulator = resolver.Resolve<IReadModelPopulator>();
        }

        [HttpPost]
        [Route("api/[controller]/rebuild")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Rebuild()
        {
            await _readModelPopulator.PopulateAsync<ExampleReadModel>(CancellationToken.None);

            return Accepted("Read models are replayed");
        }

        //[HttpPost]
        //[Route("api/[controller]/update")]
        //[ProducesResponseType(200)]
        //public async Task<ActionResult> Update()
        //{
            
        //}

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ExampleReadModel>> GetExample(string id)
        {
            var readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<ExampleReadModel>(id), CancellationToken.None);

            return Ok(readModel);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Delete()
        {
            await _readModelPopulator.PurgeAsync<ExampleReadModel>(CancellationToken.None);

            return Ok("Read models deleted");
        }
    }
}

