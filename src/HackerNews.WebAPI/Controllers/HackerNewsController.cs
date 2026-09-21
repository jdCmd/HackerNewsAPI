using HackerNews.HackerNewsApiService;
using HackerNews.HackerNewsApiService.Models;
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
        public async Task<ActionResult<HackerNewsItem>> Item(int id, CancellationToken cancellationToken)
        {
            var item = await _service.GetItemAsync(id, cancellationToken);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
