using MicroBlog.DataHandling;
using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services.Interfaces;
using MicroBlog.WebAppConfigs;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

BuilderSetUp.SetUp(builder);

var app = builder.Build();

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

app.MapGet("/test-elastic", async (IElasticSearchService elasticService) =>
{
    var mockMessageElastic = new MessageElastic
    {
        BsonId = "658c8c0a7b6122e93cbeb2b0",
        XmlId = 1,
        PostTypeId= 1,
        AcceptedAnswerId= 2,
        CreationDate= DateTime.Parse("2010-07-28T19:04:21.300"),
        Score= 62,
        ViewCount= 4484,
        Body= "&lt;p&gt;Every time I turn on my computer, I see a message saying something like:&lt;/p&gt;&#xA;&#xA;&lt;pre&gt;&lt;code&gt;Your battery may be old or broken.&#xA;&lt;/code&gt;&lt;/pre&gt;&#xA;&#xA;&lt;p&gt;I am already aware that my battery is bad. How do I suppress this message?&lt;/p&gt;&#xA;",
        OwnerUserId= 5,
        LastEditorUserId= 208574,
        LastEditDate= DateTime.Parse("2014-12-16T01:47:45.980"),
        LastActivityDate= DateTime.Parse("2018-10-05T23:56:48.997"),
        Title= "How to get the &quot;Your battery is broken&quot; message to go away?",
        Tags= "&lt;power-management&gt;&lt;notification&gt;",
        AnswerCount= 4,
        CommentCount= 2,
        ContentLicense= "CC BY-SA 3.0"
    };
    await elasticService.CreateAsync(mockMessageElastic);
});


app.MapGet("/",  () => "Hello, MicroBlog!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
    
