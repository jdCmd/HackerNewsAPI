using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.WebAPI
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred");

            var (statusCode, title) = exception switch
            {
                HttpRequestException => (StatusCodes.Status502BadGateway, "Hacker News API unavailable"),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title
            };

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });

            return true;
        }
    }
}
