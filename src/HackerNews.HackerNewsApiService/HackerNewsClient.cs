
using HackerNews.HackerNewsApiService.Models;
using Newtonsoft.Json;

namespace HackerNews.HackerNewsApiService
{
    public class HackerNewsClient : IHackerNewsClient
    {
        public async Task<HackerNewsItem?> GetItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://hacker-news.firebaseio.com/v0/item/{itemId}.json");

            using var response = await httpClient.SendAsync(request, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<HackerNewsItem?>(json);
        }
    }
}
