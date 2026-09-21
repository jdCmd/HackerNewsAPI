using HackerNews.HackerNewsApiService;
using HackerNews.HackerNewsApiService.Models;

namespace HackerNews.WebAPI.Services
{
    public class HackerNewsService(IHackerNewsClient client) : IHackerNewsService
    {
        public async Task<IReadOnlyList<HackerNewsItem>> GetBestStoriesAsync(int count, CancellationToken cancellationToken = default)
        {
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

            return stories
                .OrderByDescending(x => x.Score)
                .Take(count)
                .ToList();
        }
    }
}
