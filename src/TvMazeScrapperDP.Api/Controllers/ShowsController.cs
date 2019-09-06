using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMazeScrapperDP.Api.Models;
using TvMazeScrapperDp.Core.Models;
using TvMazeScrapperDp.Core.Services.Contracts;

namespace TvMazeScrapperDP.Api.Controllers
{
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showsService;

        public ShowsController(IShowService showsService)
        {
            _showsService = showsService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Show>))]
        public async Task<IActionResult> Get(PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _showsService.GetAllPaginatedAsync(request.Page, request.PageSize, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Show))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var maybeShow = await _showsService.GetShowAsync(id, cancellationToken);

            return maybeShow.HasValue ? (IActionResult)Ok(maybeShow.Value) : NotFound();
        }
    }
}