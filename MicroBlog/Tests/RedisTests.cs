using System.Text.Json;
using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit.Abstractions;

namespace Tests;

public class RedisTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RedisTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void SetAndGet()
    {
        var app = WebAppForTest.GetTestApp();
        var redisDb = app.Services.GetRequiredService<IRedisService>().GetRedisDb();

        var pair1 = new { key = "Ivan2", value = "Petrov" };
        var pair2 = new { key = "Ivan12", value = "Petrov2" };
        
        redisDb.StringSet(pair1.key, pair1.value);
        redisDb.StringSet(pair2.key, pair2.value);
        
        Assert.True(redisDb.StringGet(pair1.key).HasValue);
        Assert.Equal(pair1.value, redisDb.StringGet(pair1.key).ToString());
        
        Assert.True(redisDb.StringGet(pair2.key).HasValue);
        Assert.Equal(pair2.value, redisDb.StringGet(pair2.key).ToString());
        
        // deleting
        redisDb.KeyDelete(pair1.key);
        redisDb.KeyDelete(pair2.key);
    }
    
    [Fact]
    public async void GetFromMongoGetFromRedis()
    {
        var app = WebAppForTest.GetTestApp();
        // get dependencies
        var redisDb = app.Services.GetRequiredService<IRedisService>().GetRedisDb();
        var mongoDbService = app.Services.GetRequiredService<IMongoDbService>();
        var databaseSettings = app.Services.GetRequiredService<IOptions<MicroBlogDatabaseSettings>>();
        var userService = app.Services.GetRequiredService<IUserAccountsService>();
        
        var collectionName = databaseSettings.Value.UserAccountsCollectionName;
        var userAccountsCollection = mongoDbService.MongoDatabase.GetCollection<UserAccount>(collectionName);
        
        // insert mock document into the mongo db
        
        var mockUserAccount = new UserAccount
        {
            XmlId = 422,
            Reputation = 400,
            CreationDate = "2011-07-28T16:38:27.683",
            DisplayName = "CommunityMock",
            LastAccessDate = "2010-07-28T16:38:27.683",
            WebsiteUrl = "https://meta.stackexchange.com/",
            Location = "some location on the earth",
            AboutMe = "&lt;p&gt;Hi, I'm not really a person.&lt;/p&gt;&#xA;&lt;p&gt;I'm a background process that helps keep this site clean!&lt;/p&gt;&#xA;&lt;p&gt;I do things like&lt;/p&gt;&#xA;&lt;ul&gt;&#xA;&lt;li&gt;Randomly poke old unanswered questions every hour so they get some attention&lt;/li&gt;&#xA;&lt;li&gt;Own community questions and answers so nobody gets unnecessary reputation from them&lt;/li&gt;&#xA;&lt;li&gt;Own downvotes on spam/evil posts that get permanently deleted&lt;/li&gt;&#xA;&lt;li&gt;Own suggested edits from anonymous users&lt;/li&gt;&#xA;&lt;li&gt;&lt;a href=&quot;https://meta.stackexchange.com/a/92006&quot;&gt;Remove abandoned questions&lt;/a&gt;&lt;/li&gt;&#xA;&lt;/ul&gt;&#xA;",
            Views = 15831,
            UpVotes = 25241,
            DownVotes = 217531,
            AccountId = 31231
        };
        await userAccountsCollection.InsertOneAsync(mockUserAccount);
        
        var id = mockUserAccount.Id;
        // first try to get the mock user account
        UserAccount? userAccount = null;
        // try to get userAccount from the redis cache
        var userAccountRedisValue = await redisDb.StringGetAsync(id);
        Assert.True(userAccountRedisValue.IsNull);
        // get from the mongo db
        userAccount = await userAccountsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        Assert.True(userAccount != null);
        // set to redis
        var userAccountString = JsonSerializer.Serialize(userAccount);
        await redisDb.StringSetAsync(id, userAccountString);
        
        // try again to get from the redis
        userAccountRedisValue = await redisDb.StringGetAsync(id);
        Assert.True(!userAccountRedisValue.IsNull);
        userAccount = JsonSerializer.Deserialize<UserAccount>(userAccountRedisValue.ToString());
        
        // check some fields
        Assert.Equal(userAccount?.Id, mockUserAccount.Id);
        Assert.Equal(userAccount?.Reputation, mockUserAccount.Reputation);
        Assert.Equal(userAccount?.DisplayName, mockUserAccount.DisplayName);
        
        // deleting
        await userAccountsCollection.DeleteOneAsync(x => x.Id == mockUserAccount.Id);
        await redisDb.KeyDeleteAsync(id);
    }
}