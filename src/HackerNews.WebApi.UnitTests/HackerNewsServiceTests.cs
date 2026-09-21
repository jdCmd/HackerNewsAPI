using FluentAssertions;
using HackerNews.HackerNewsApiService;
using HackerNews.HackerNewsApiService.Models;
using HackerNews.WebAPI.Services;
using Moq;
using NUnit.Framework;

namespace HackerNews.WebApi.UnitTests
{
    [TestFixture]
    public class HackerNewsServiceTests
    {
        private Mock<IHackerNewsClient> _hackerNewsClientMock = null!;

        [SetUp]
        public void Setup()
        {
            _hackerNewsClientMock = new Mock<IHackerNewsClient>();
        }

        [Test]
        public async Task GetBestStoriesAsync_ClientThorwsHttpException_ThrowsHttpException()
        {
            // Arrange
            var ex = new HttpRequestException();
            
            _hackerNewsClientMock.Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(ex);

            var sut = GetSut();

            // Act
            var action = () => sut.GetBestStoriesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>());

            // Assert
            var result = await action.Should().ThrowAsync<HttpRequestException>();
            result.Which.Should().Be(ex);

            _hackerNewsClientMock.Verify(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _hackerNewsClientMock.Verify(x => x.GetItemAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task GetBestStoriesAsync_ClientReturnsEmpty_ReturnsEmptyList()
        {
            // Arrange
            _hackerNewsClientMock.Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new int[0]);

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStoriesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>());

            // Assert
            result.Should().BeEmpty();

            _hackerNewsClientMock.Verify(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _hackerNewsClientMock.Verify(x => x.GetItemAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task GetBestStoriesAsync_ClientReturnsNullStoryForId_ReturnsEmptyList()
        {
            // Arrange
            var id = 1;
            var count = 1;

            _hackerNewsClientMock.Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new int[] { id });
            _hackerNewsClientMock.Setup(x => x.GetItemAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((HackerNewsItem?)null);

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStoriesAsync(count, It.IsAny<CancellationToken>());

            // Assert
            result.Should().BeEmpty();

            _hackerNewsClientMock.Verify(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _hackerNewsClientMock.Verify(x => x.GetItemAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetBestStoriesAsync_MultipleStoriesInScoreOrder_ReturnsStoriesOrderedByScore()
        {
            // Arrange
            var count = 3;
            var stories = new[]
            {
                CreateItem(1, 100),
                CreateItem(2, 75),
                CreateItem(3, 50)
            };

            _hackerNewsClientMock
                .Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stories.Select(x => x.Id).ToArray());

            foreach (var story in stories)
            {
                _hackerNewsClientMock
                    .Setup(x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(story);
            }

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStoriesAsync(count, CancellationToken.None);

            // Assert
            result.Select(x => x.Id).Should().Equal(1, 2, 3);
            result.Should().HaveCount(count);

            _hackerNewsClientMock.Verify(
                x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            foreach (var story in stories)
            {
                _hackerNewsClientMock.Verify(
                    x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Test]
        public async Task GetBestStoriesAsync_MultipleStoriesOutOfScoreOrder_ReturnsStoriesOrderedByScore()
        {
            // Arrange
            var count = 3;
            var stories = new[]
            {
                CreateItem(1, 50),
                CreateItem(2, 100),
                CreateItem(3, 75)
            };

            _hackerNewsClientMock
                .Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stories.Select(x => x.Id).ToArray());

            foreach (var story in stories)
            {
                _hackerNewsClientMock
                    .Setup(x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(story);
            }

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStoriesAsync(count, CancellationToken.None);

            // Assert
            result.Select(x => x.Id).Should().Equal(2, 3, 1);
            result.Should().HaveCount(count);

            _hackerNewsClientMock.Verify(
                x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            foreach (var story in stories)
            {
                _hackerNewsClientMock.Verify(
                    x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Test]
        public async Task GetBestStoriesAsync_CountLessThanStoryCount_ReturnsSubSetOfStories()
        {
            // Arrange
            var count = 2;
            var stories = new[]
            {
                CreateItem(1, 50),
                CreateItem(2, 100),
                CreateItem(3, 75)
            };

            _hackerNewsClientMock
                .Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stories.Select(x => x.Id).ToArray());

            foreach (var story in stories)
            {
                _hackerNewsClientMock
                    .Setup(x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(story);
            }

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStoriesAsync(count, CancellationToken.None);

            // Assert
            result.Select(x => x.Id).Should().Equal(2, 3);
            result.Should().HaveCount(count);

            _hackerNewsClientMock.Verify(
                x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            foreach (var story in stories)
            {
                _hackerNewsClientMock.Verify(
                    x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Test]
        public async Task GetBestStoriesAsync_CountGreaterThanStoryCount_ReturnsAllStories()
        {
            // Arrange
            var count = 10;
            var stories = new[]
            {
                CreateItem(1, 50),
                CreateItem(2, 100),
                CreateItem(3, 75)
            };

            _hackerNewsClientMock
                .Setup(x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(stories.Select(x => x.Id).ToArray());

            foreach (var story in stories)
            {
                _hackerNewsClientMock
                    .Setup(x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(story);
            }

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStoriesAsync(count, CancellationToken.None);

            // Assert
            result.Select(x => x.Id).Should().Equal(2, 3, 1);
            result.Should().HaveCount(3);

            _hackerNewsClientMock.Verify(
                x => x.GetBestStoriesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            foreach (var story in stories)
            {
                _hackerNewsClientMock.Verify(
                    x => x.GetItemAsync(story.Id, It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        private HackerNewsService GetSut()
        {
            return new HackerNewsService(_hackerNewsClientMock.Object);
        }

        private static HackerNewsItem CreateItem(int id, int score)
            => new(
                id,
                false,
                "story",
                "user",
                123456789,
                null,
                false,
                null,
                null,
                null,
                "https://example.com",
                score,
                $"Story {id}",
                null,
                0);
    }
}
