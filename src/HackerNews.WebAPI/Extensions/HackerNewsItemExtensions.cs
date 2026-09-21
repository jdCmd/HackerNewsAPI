using HackerNews.HackerNewsApiService.Models;
using HackerNews.WebAPI.Dtos;

namespace HackerNews.WebAPI.Extensions
{
    public static class HackerNewsItemExtensions
    {
        public static HackerNewsItemDto ToDto(this HackerNewsItem item)
        {
            return new HackerNewsItemDto(
                item.Id, 
                item.Deleted, 
                item.Type, 
                item.By, 
                item.Time, 
                item.Text, 
                item.Dead, 
                item.Parent, 
                item.Poll, 
                item.Kids, 
                item.Url, 
                item.Score, 
                item.Title, 
                item.Parts, 
                item.Descendants);
        }
    }
}
