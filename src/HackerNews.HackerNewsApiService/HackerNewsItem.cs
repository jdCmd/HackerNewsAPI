namespace HackerNews.HackerNewsApiService
{
    public record HackerNewsItem(
        long Id,
        bool? Deleted,
        string? Type,
        string? By,
        long? Time,
        string? Text,
        bool? Dead,
        long? Parent,
        long? Poll,
        List<long>? Kids,
        string? Url,
        int? Score,
        string? Title,
        List<long>? Parts,
        int? Descendants
    );
}
