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

        /// <summary>
        /// Rebuilds all data models
        /// </summary>
        /// <remarks>The request will be of /api/DataModels/Rebuild</remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        // [Route("rebuild")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Rebuild()
        {
            await _readModelPopulator.PopulateAsync<ExampleReadModel>(CancellationToken.None);
            await _readModelPopulator.PopulateAsync<AnotherExampleReadModel>(CancellationToken.None);

            return Accepted("All Read models are replayed");
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Delete()
        {
            await _readModelPopulator.PurgeAsync<ExampleReadModel>(CancellationToken.None);
            await _readModelPopulator.PurgeAsync<AnotherExampleReadModel>(CancellationToken.None);

            return Ok("All Read models deleted");
        }
    }
}

