namespace HackerNews.WebAPI.Dtos
{
    public record HackerNewsItemDto(
        string? Title,
        string? Url,
        string? PostedBy,
        string? Time,
        int? Score,
        int? CommentCount
    );
}
