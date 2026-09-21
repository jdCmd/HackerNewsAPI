using HackerNews.WebAPI.Dtos;
using HackerNews.WebAPI.Extensions;
using HackerNews.WebAPI.Services;
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

        [HttpGet]
        [Route("bestStories")]
        public async Task<ActionResult<HackerNewsItemDto[]>> GetBestStories(int count, CancellationToken cancellationToken)
        {
            if (count <= 0)
            {
                _logger.LogWarning("{Method} failed. Invalid count requested: {Count}", nameof(GetBestStories), count);
                return BadRequest("count parameter must be 1 or greater");
            }

            _logger.LogInformation("{Method} called. Retrieving {Count} best stories", nameof(GetBestStories), count);

            var stories = await _service.GetBestStoriesAsync(count, cancellationToken);
            var storiesArray = stories.Select(x => x.ToDto()).ToArray();

            _logger.LogInformation("{Method} succeeded. Retrieved {Count} best stories", nameof(GetBestStories), storiesArray.Length);

            return Ok(storiesArray);
        }
    }
}
