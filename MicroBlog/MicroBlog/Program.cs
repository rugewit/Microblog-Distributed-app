using MicroBlog.DataHandling;
using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// mongo db configuration
builder.Services.Configure<MicroBlogDatabaseSettings>(
    builder.Configuration.GetSection("MicroBlogDatabase"));
// datasets path configuration
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
    
    app.MapGet("/load-users", async (IUserAccountsService userAccountsService, 
        IOptions<DatasetPathSettings> datasetPath) =>
    {
        await DatasetLoader.LoadUsers(userAccountsService, datasetPath);
    });

    app.MapGet("/load-messages", async (IMessagesService messagesService, 
        IOptions<DatasetPathSettings> datasetPath) =>
    {
        await DatasetLoader.LoadMessages(messagesService, datasetPath);
    });

    app.MapGet("/redis",  async (IRedisService redisService) =>
    {
        var redisDb = redisService.GetRedisDb();
        await redisDb.StringSetAsync("Ivan", "Petrov");
        await redisDb.StringSetAsync("Ivan1", "Petrov2");
    });
}

app.MapGet("/",  () => "Hello, MicroBlog!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();