
using FluentAssertions;
using NUnit.Framework;

namespace HackerNews.HackerNewsApiService.Tests
{
    [TestFixture]
    public class HackerNewsClientTestFixture
    {
        // test the examples provided in the HackerAPI docs here: https://github.com/HackerNews/API
        [Test]
        [TestCase(8863)]
        [TestCase(2921983)]
        [TestCase(121003)]
        [TestCase(192327)]
        [TestCase(126809)]
        [TestCase(160705)]
        public async Task GetItemAsync_GivenExampleId_ReturnsExpected(int id)
        {
            // Arrange
            var client = new HackerNewsClient();

            // Act
            var result = await client.GetItemAsync(id);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        [TestCase(0)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public async Task GetItemAsync_NoItemWithGivenId_ReturnsNull(int id)
        {
            // Arrange
            var client = new HackerNewsClient();

            // Act
            var result = await client.GetItemAsync(id);

            // Assert
            result.Should().BeNull();
        }
    }
}
