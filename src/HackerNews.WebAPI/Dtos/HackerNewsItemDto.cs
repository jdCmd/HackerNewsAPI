namespace HackerNews.WebAPI.Dtos
{
    public record HackerNewsItemDto(
        int Id,
        bool? Deleted,
        string? Type,
        string? By,
        long? Time,
        string? Text,
        bool? Dead,
        int? Parent,
        int? Poll,
        List<int>? Kids,
        string? Url,
        int? Score,
        string? Title,
        List<int>? Parts,
        int? Descendants
    );
}
