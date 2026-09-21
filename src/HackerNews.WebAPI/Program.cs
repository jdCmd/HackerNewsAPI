using HackerNews.HackerNewsApiService;
using HackerNews.WebAPI;
using HackerNews.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHackerNewsService, HackerNewsService>();
builder.Services.AddSingleton<IHackerNewsClient, HackerNewsClient>();
builder.Services.AddHttpClient<IHackerNewsClient, HackerNewsClient>(client => client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/"));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();
