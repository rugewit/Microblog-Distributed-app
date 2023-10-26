using MicroBlog.DataHandling;
using MicroBlog.Models.Settings;
using MicroBlog.Services.Interfaces;
using MicroBlog.WebAppConfigs;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

BuilderSetUp.SetUp(builder);

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
        
        var res1 = await redisDb.StringGetAsync("Ivan");
        var res2 = await redisDb.StringGetAsync("Ivan1");
        return $"Ivan={res1}\nIvan1={res2}";
    });
}

app.MapGet("/",  () => "Hello, MicroBlog!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();