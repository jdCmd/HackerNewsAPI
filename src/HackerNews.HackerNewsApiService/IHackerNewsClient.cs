using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.HackerNewsApiService
{
    public interface IHackerNewsClient
    {
        Task<HackerNewsItem?> GetItemAsync(int itemId, CancellationToken cancellationToken = default);
    }
}
