using FluentAssertions;
using HackerNews.HackerNewsApiService;
using HackerNews.WebAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace HackerNews.WebApi.IntegrationTests
{
    [TestFixture]
    public class HackerNewsControllerTests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public async Task GetBestStories_InvalidCount_ReturnsBadRequest(int count)
        {
            // Act
            var response = await _client.GetAsync($"/hackernews/bestStories?count={count}");

            // Assert
            AssertErrorResponseIsAsExpected(
                response,
                HttpStatusCode.BadRequest,
                StatusCodes.Status400BadRequest,
                "Invalid request",
                "count parameter must be 1 or greater");
        }

        [Test]
        public async Task GetBestStories_WithValidCount_ReturnsOk()
        {
            // Arrange
            var count = 3;

            // Act
            var response = await _client.GetAsync($"/hackernews/bestStories?count={count}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var items = await response.Content
                .ReadFromJsonAsync<HackerNewsItemDto[]>();
            items.Should().HaveCount(count);            
        }
                
        [Test]
        public async Task GetBestStories_HackerNewsCallReturnsFailureStatusCode_ReturnsBadGatewayProblemDetails()
        {
            // Arrange
            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            using var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services
                            .AddHttpClient<IHackerNewsClient, HackerNewsClient>()
                            .ConfigurePrimaryHttpMessageHandler(() => handler.Object);
                    });
                });

            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/hackernews/bestStories?count=10");

            // Assert
            AssertErrorResponseIsAsExpected(
                response,
                HttpStatusCode.BadGateway,
                StatusCodes.Status502BadGateway,
                "Hacker News API unavailable",
                null);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        private static void AssertErrorResponseIsAsExpected(HttpResponseMessage response, HttpStatusCode expectedHttpStatusCode, int expectedStatusCode, string expectedTitle, string? expectedDetail)
        {
            response.StatusCode.Should().Be(expectedHttpStatusCode);

            var problemDetails = response.Content.ReadFromJsonAsync<ProblemDetails>().Result;
            problemDetails.Should().NotBeNull();
            problemDetails!.Status.Should().Be(expectedStatusCode);
            problemDetails.Title.Should().Be(expectedTitle);
            problemDetails.Detail.Should().Be(expectedDetail);
        }
    }
}
