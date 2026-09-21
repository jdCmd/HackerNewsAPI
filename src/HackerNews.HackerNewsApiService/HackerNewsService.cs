
using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.HackerNewsApiService
{
    public class HackerNewsService(IHackerNewsClient client) : IHackerNewsService
    {
        public async Task<HackerNewsItem?> GetItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            var item = await client.GetItemAsync(itemId, cancellationToken);

            // todo add caching here

            return item;
        }
    }
}
