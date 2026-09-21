
using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.HackerNewsApiService
{
    public interface IHackerNewsService
    {
        Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default);
    }
}
