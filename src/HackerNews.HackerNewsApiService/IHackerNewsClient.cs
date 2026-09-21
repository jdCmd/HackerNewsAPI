namespace HackerNews.HackerNewsApiService
{
    public interface IHackerNewsClient
    {
        Task<HackerNewsItem?> GetStoryAsync(int storyId, CancellationToken cancellation);
    }
}
