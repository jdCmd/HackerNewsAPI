using FluentAssertions;
using HackerNews.HackerNewsApiService.Models;
using HackerNews.WebAPI.Extensions;
using NUnit.Framework;

namespace HackerNews.WebApi.UnitTests
{
    [TestFixture]
    public class HackerNewsItemExtensionsTests
    {
        [Test]
        public void ToDto_MapsAllProperties()
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

            // Act
            var result = item.ToDto();

            // Assert
            result.Should().BeEquivalentTo(item);
        }
    }
}
