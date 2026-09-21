using HackerNews.HackerNewsApiService;
using HackerNews.WebAPI.Dtos;
using HackerNews.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly ILogger<HackerNewsController> _logger;
        private readonly IHackerNewsService _service;

        public HackerNewsController(ILogger<HackerNewsController> logger, IHackerNewsService service)
        {
            _logger = logger;
            _service = service;
        }

        // todo exception handling
        // logging
        // cache
        // comments
        [HttpGet]
        [Route("bestStories")]
        public async Task<ActionResult<HackerNewsItemDto[]>> BestStories(int count, CancellationToken cancellationToken)
        {
            var stories = await _service.GetBestStoriesAsync(count, cancellationToken);

            if (stories is null)
            {
                return NotFound();
            }


            return Ok(stories.Select(x => x.ToDto()));
        }
    }
}
