using HackerNews.HackerNewsApiService;
using HackerNews.HackerNewsApiService.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.WebAPI.Services
{
    public class HackerNewsService(IHackerNewsClient client, IMemoryCache cache) : IHackerNewsService
    {
        private const string cacheKey = "hacker-news-best-stories";
        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1);

        public async Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default)
        {
            if (TryGetStoriesFromCache(out var cachedStories))
            {
                return cachedStories!.Take(count).ToList();
            }

            try
            {
                await _cacheLock.WaitAsync(cancellationToken);

                if (TryGetStoriesFromCache(out cachedStories))
                {
                    return cachedStories!.Take(count).ToList();
                }

                // In the docs there is no mention of the ids returned by the beststories endpoint being ordered. 
                // Hence the assumption this is not the case.
                var bestStoryIds = await client.GetBestStoriesAsync(cancellationToken);
                var tasks = bestStoryIds.Select(storyId => client.GetItemAsync(storyId, cancellationToken));
                var stories = await Task.WhenAll(tasks);

                var orderedStories = stories
                    .Where(x => x is not null)
                    .Select(x => x!)
                    .OrderByDescending(x => x.Score)
                    .ToList();

                cache.Set(cacheKey, orderedStories, TimeSpan.FromMinutes(5));
                return orderedStories.Take(count).ToList();
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        private bool TryGetStoriesFromCache(
            out List<HackerNewsItem>? cachedStories)
        {
            return cache.TryGetValue(cacheKey, out cachedStories)
                   && cachedStories is not null;
        }
    }
}
