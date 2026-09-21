using HackerNews.HackerNewsApiService.Models;
using HackerNews.WebAPI.Dtos;

namespace HackerNews.WebAPI.Extensions
{
    public static class HackerNewsItemExtensions
    {
        public static HackerNewsItemDto ToDto(this HackerNewsItem item)
        {
            return new HackerNewsItemDto(
                item.Title,
                item.Url, 
                item.By,
                item.Time == null ? null : DateTimeOffset.FromUnixTimeSeconds(item.Time.Value).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
                item.Score,
                item.Descendants);
        }
    }
}
