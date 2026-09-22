# HackerNewsAPI

# Hacker News API

ASP.NET Core Web API that retrieves the best N stories from the Hacker News API.

## Prerequisites

* .NET SDK 8.0 or later installed
* Internet access, as the application calls the Hacker News API

## Running the API

Clone the repository. Open a command prompt in the `src` folder:

```bash
cd ./src
```

Restore the NuGet packages and build the solution

```bash
dotnet restore
dotnet build
```

Run the application:

```bash
cd ./HackerNews.WebAPI
dotnet run
```

The API will start using the configured ASP.NET Core URLs. The URLs will be listed in the command window.

### Swagger

When running in the **Development** environment, Swagger UI is available under the `/swagger` endpoint.

For example:

```text
http://localhost:5000/swagger
```

Swagger can be used to execute the available API endpoints directly from the browser.

### Get Best Stories

Send a GET request to:

```text
GET /bestStories?count=10
```

For example, using `curl`:

```bash
curl "http://localhost:5000/hackernews/bestStories?count=10"
```

Replace the port with the port shown when the application starts.

The `count` parameter must be greater than zero.

Example response:

```json
[
  {
    "title": "MiMo v2.6",
    "url": "https://mimo.xiaomi.com/mimo-v2-6",
    "postedBy": "volf_",
    "time": "2026-09-21T20:12:12+00:00",
    "score": 755,
    "commentCount": 345
  }
]
```

## Running the Tests

From the src directory:

```bash
dotnet test
```

This runs the unit tests for the API, service, and Hacker News client.

## Implementation Notes

* Best story IDs are retrieved from the Hacker News API.
* Story details are retrieved concurrently.
* Concurrent cache population is protected to prevent multiple requests from refreshing the cache simultaneously.
* External API failures are handled by the global exception handler.

## What I Would Add With More Time

The following would be potential improvements for a production-ready version:

* **XML documentation** — Add XML documentation to public APIs, services, DTOs, and client methods, and expose the documentation through Swagger.
* **Docker support** — Add a `Dockerfile` and potentially a `docker-compose.yml` to make the application easier to build and run consistently across environments.
* **Configuration** — Move settings such as the Hacker News API base URL and cache duration into configuration rather than keeping them in code.
* **Resilience** — Add retry and timeout policies for transient Hacker News API failures, for example using Polly.
* **Request throttling** — Introduce bounded concurrency when retrieving a large number of stories to avoid sending too many requests to the upstream API at once.
* **Integration tests** — Add tests covering the HTTP API and the integration with the Hacker News client in addition to the unit tests.
* **CI/CD** — Add a CI/CD pipeline to automatically build and test the application
* **Production observability** — Add more structured logging. Add metrics for observability - eg. request duration, memory, CPU Utilisation, failure count.
* **Cache improvements** — Consider a distributed cache such as Redis if the API were deployed across multiple instances, since `IMemoryCache` is local to each application instance.
