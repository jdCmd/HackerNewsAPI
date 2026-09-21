
using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.HackerNewsApiService
{
    public class HackerNewsService(IHackerNewsClient client) : IHackerNewsService
    {
        public async Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default)
        {
            var bestStoryIds = await client.GetBestStoriesAsync(cancellationToken);

            var stories = new List<HackerNewsItem>();

            // In the docs there is no mention of the ids returned by the beststories endpoint being ordered. 
            // So to be safe I have not assumed this is the case. Of course this means a large number of calls are made.
            foreach (var storyId in bestStoryIds)
            {
                var story = await client.GetItemAsync(storyId, cancellationToken);

                if (story is not null)
                {
                    stories.Add(story);
                }
            }

            return stories
                .OrderByDescending(x => x.Score)
                .Take(count)
                .ToList();
        }

        public async Task<HackerNewsItem?> GetItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            var item = await client.GetItemAsync(itemId, cancellationToken);

            // todo add caching here

            return item;
        }
    }
}
