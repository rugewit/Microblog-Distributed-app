using MicroBlog.DataHandling;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services.Interfaces;
using MicroBlog.WebAppConfigs;
using Microsoft.Extensions.Options;

namespace MicroBlog;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hey, I am running");
        var builder = WebApplication.CreateBuilder(args);

        BuilderSetUp.SetUp(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
    
            app.MapGet("/load-users-db", async (IUserAccountsService userAccountsService, 
                IOptions<DatasetPathSettings> datasetPath, ILogger<Program> logger) =>
            {
                await DatasetLoader.LoadUsersIntoDb(userAccountsService, datasetPath, logger);
            });

            app.MapGet("/load-messages-db", async (IMessagesService messagesService,
                IElasticSearchService elasticSearchService,
                IOptions<DatasetPathSettings> datasetPath, ILogger<Program> logger) =>
            {
                await DatasetLoader.LoadMessagesIntoDb(messagesService, elasticSearchService, datasetPath, logger);
            });
    
            app.MapGet("/load-messages-elastic", async (IMessagesService messagesService,
                IElasticSearchService elasticSearchService,
                IOptions<DatasetPathSettings> datasetPath, ILogger<Program> logger) =>
            {
                await DatasetLoader.LoadMessagesIntoElastic(messagesService, elasticSearchService, datasetPath, logger);
            });

            app.MapGet("/redis",  async (IRedisProvider redisService) =>
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
    }
}