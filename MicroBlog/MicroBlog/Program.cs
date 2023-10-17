using MicroBlog.DataHandling;
using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MicroBlogDatabaseSettings>(
    builder.Configuration.GetSection("MicroBlogDatabase"));
builder.Services.Configure<DatasetPathSettings>(
    builder.Configuration.GetSection("DatasetPath"));

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<UserAccountsService>();
builder.Services.AddSingleton<MessagesService>();
builder.Services.AddSingleton(new RedisService("localhost"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/load-users", async (UserAccountsService userAccountsService, 
    IOptions<DatasetPathSettings> datasetPath) =>
{
    await DatasetLoader.LoadUsers(userAccountsService, datasetPath);
});

app.MapGet("/load-messages", async (MessagesService messagesService, 
    IOptions<DatasetPathSettings> datasetPath) =>
{
    await DatasetLoader.LoadMessages(messagesService, datasetPath);
});

app.MapGet("/redis",  async (RedisService redisService) =>
{
    var redisDb = redisService.GetRedisDB();
    await redisDb.StringSetAsync("Ivan", "Petrov");
});

app.MapGet("/",  () => "Hello, MicroBlog!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();