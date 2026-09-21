
using HackerNews.HackerNewsApiService.Models;
using Newtonsoft.Json;

namespace HackerNews.HackerNewsApiService
{
    public class HackerNewsClient(HttpClient httpClient) : IHackerNewsClient
    {        
        public async Task<HackerNewsItem?> GetItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<HackerNewsItem>(HttpMethod.Get, $"item/{itemId}.json", cancellationToken);
        }

        public async Task<int[]?> GetBestStoriesAsync(CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<int[]>(HttpMethod.Get, $"beststories.json", cancellationToken);
        }

        private async Task<TResult?> MakeRequestAsync<TResult>(HttpMethod httpMethod, string endpoint, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(httpMethod, endpoint);
            
            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<TResult>(json);
        }
    }
}
