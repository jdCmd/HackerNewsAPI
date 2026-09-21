using FluentAssertions;
using HackerNews.HackerNewsApiService.Models;
using HackerNews.WebAPI.Dtos;
using HackerNews.WebAPI.Extensions;
using NUnit.Framework;

namespace HackerNews.WebApi.UnitTests
{
    [TestFixture]
    public class HackerNewsItemExtensionsTests
    {
        [Test]
        public void ToDto_MapsPropertiesAsExpected()
        {
            // Arrange
            var item = new HackerNewsItem(
                123,
                false,
                "story",
                "user",
                123456789,
                "Some text",
                false,
                456,
                789,
                [1, 2, 3],
                "https://example.com",
                42,
                "Some title",
                [10, 20],
                5);

            var expected = new HackerNewsItemDto(
                item.Title,
                item.Url,
                item.By,
                "1973-11-29T21:33:09+00:00",
                item.Score,
                item.Descendants);

            // Act
            var result = item.ToDto();

            // Assert
            result.Should().BeEquivalentTo(item);
        }
    }
}
