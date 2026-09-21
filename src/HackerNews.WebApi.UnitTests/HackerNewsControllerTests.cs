using FluentAssertions;
using HackerNews.HackerNewsApiService;
using HackerNews.HackerNewsApiService.Models;
using HackerNews.WebAPI.Controllers;
using HackerNews.WebAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HackerNews.WebApi.UnitTests
{
    [TestFixture]
    public class HackerNewsControllerTests
    {
        private Mock<IHackerNewsService> _hackerNewsServiceMock = null!;
        private Mock<ILogger<HackerNewsController>> _loggerMock = null!;

        [SetUp]
        public void Setup()
        {
            _hackerNewsServiceMock = new Mock<IHackerNewsService>();
            _loggerMock = new Mock<ILogger<HackerNewsController>>();
        }

        [Test]
        public async Task GetBestStories_CountLessThan1_ReturnsBadRequest()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var result = await sut.GetBestStories(0, It.IsAny<CancellationToken>());

            // Assert
            var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            actionResult.Value.Should().Be("count parameter must be 1 or greater");

            _hackerNewsServiceMock.Verify(x => x.GetBestStoriesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task GetBestStories_ServiceReturnsEmptyItems_ReturnsOkWithEmptyStoriesArray()
        {
            // Arrange
            var count = 1;
            var expectedDtos = new HackerNewsItemDto[0];

            _hackerNewsServiceMock.Setup(x => x.GetBestStoriesAsync(count, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HackerNewsItem[0]);

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStories(count, It.IsAny<CancellationToken>());

            // Assert
            var actionResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            actionResult.Value.Should().BeEquivalentTo(expectedDtos);

            _hackerNewsServiceMock.Verify(x => x.GetBestStoriesAsync(count, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetBestStories_ServiceReturnsItems_ReturnsOkWithExpectedStoriesArray()
        {
            // Arrange
            var count = 1;
            var items = new[]
            {
                new HackerNewsItem(
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
                    5) 
            };

            var expectedDtos = new[] 
            {
                new HackerNewsItemDto(
                    "Some title",
                    "https://example.com",
                    "user",
                    "1973-11-29T21:33:09+00:00",
                    42,
                    5) 
            };


            _hackerNewsServiceMock.Setup(x => x.GetBestStoriesAsync(count, It.IsAny<CancellationToken>()))
                .ReturnsAsync(items);

            var sut = GetSut();

            // Act
            var result = await sut.GetBestStories(count, It.IsAny<CancellationToken>());

            // Assert
            var actionResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            actionResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            actionResult.Value.Should().BeEquivalentTo(expectedDtos);

            _hackerNewsServiceMock.Verify(x => x.GetBestStoriesAsync(count, It.IsAny<CancellationToken>()), Times.Once);
        }

        private HackerNewsController GetSut()
        {
            return new HackerNewsController(_loggerMock.Object, _hackerNewsServiceMock.Object);
        }
    }
}
