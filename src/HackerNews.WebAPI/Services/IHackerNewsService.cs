using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.WebAPI.Services
{
    public interface IHackerNewsService
    {
        Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default);
    }
}
