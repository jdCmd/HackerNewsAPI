
using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.HackerNewsApiService
{
    public interface IHackerNewsService
    {
        Task<HackerNewsItem?> GetItemAsync(int id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default);
    }
}
