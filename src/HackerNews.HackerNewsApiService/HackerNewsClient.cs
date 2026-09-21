
using HackerNews.HackerNewsApiService.Models;
using Newtonsoft.Json;

namespace HackerNews.HackerNewsApiService
{
    public class HackerNewsClient(HttpClient httpClient) : IHackerNewsClient
    {        
        public async Task<HackerNewsItem?> GetItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"item/{itemId}.json");

            using var response = await httpClient.SendAsync(request, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<HackerNewsItem?>(json);
        }
    }
}
