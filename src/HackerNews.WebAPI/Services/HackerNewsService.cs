using HackerNews.HackerNewsApiService;
using HackerNews.HackerNewsApiService.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.WebAPI.Services
{
    public class HackerNewsService(IHackerNewsClient client, IMemoryCache cache) : IHackerNewsService
    {
        private const string cacheKey = "hacker-news-best-stories";

        public async Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default)
        {
            if (cache.TryGetValue(cacheKey, out List<HackerNewsItem>? cachedStories) && cachedStories is not null)
            {
                return cachedStories.Take(count).ToList();
            }

            var bestStoryIds = await client.GetBestStoriesAsync(cancellationToken);
            var stories = new List<HackerNewsItem>();

            // In the docs there is no mention of the ids returned by the beststories endpoint being ordered. 
            // Hence the assumption this is not the case.
            foreach (var storyId in bestStoryIds)
            {
                var story = await client.GetItemAsync(storyId, cancellationToken);

                if (story is not null)
                {
                    stories.Add(story);
                }
            }

            var orderedStories = stories.OrderByDescending(x => x.Score).ToList();
            cache.Set(cacheKey, orderedStories, TimeSpan.FromMinutes(5));
            return orderedStories.Take(count).ToList();
        }
    }
}
