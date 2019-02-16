using System.Threading;
using System.Threading.Tasks;
using Common;
using EventFlow.Configuration;
using EventFlow.ReadStores;
using Microsoft.AspNetCore.Mvc;

namespace ProjectionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataModelController : ControllerBase
    {
        private readonly IReadModelPopulator _readModelPopulator;

        public DataModelController(IResolver resolver)
        {
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

        [HttpPost]
        [Route("api/[controller]/update")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Update()
        {
            
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
}
