using MicroBlog.Controllers;
using MicroBlog.Models;
using MicroBlog.Providers;
using MicroBlog.Providers.Interfaces;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Tests;

public class UserAccountsRestTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UserAccountsRestTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void PostGetDelete()
    {
        var app = WebAppForTest.GetTestApp();

        var userService = app.Services.GetRequiredService<IUserAccountsService>();
        var redisDb = app.Services.GetRequiredService<IRedisProvider>().GetRedisDb();
        
        var controller = new UserAccountsController(userService);

        var mockUserAccount = new UserAccount
        {
            XmlId = 322,
            Reputation = 200,
            CreationDate = DateTime.Parse("2010-07-28T16:38:27.683"),
            DisplayName = "CommunityMock",
            LastAccessDate = DateTime.Parse("2010-07-28T16:38:27.683"),
            WebsiteUrl = "https://meta.stackexchange.com/",
            Location = "some location on the earth",
            AboutMe = "&lt;p&gt;Hi, I'm not really a person.&lt;/p&gt;&#xA;&lt;p&gt;I'm a background process that helps keep this site clean!&lt;/p&gt;&#xA;&lt;p&gt;I do things like&lt;/p&gt;&#xA;&lt;ul&gt;&#xA;&lt;li&gt;Randomly poke old unanswered questions every hour so they get some attention&lt;/li&gt;&#xA;&lt;li&gt;Own community questions and answers so nobody gets unnecessary reputation from them&lt;/li&gt;&#xA;&lt;li&gt;Own downvotes on spam/evil posts that get permanently deleted&lt;/li&gt;&#xA;&lt;li&gt;Own suggested edits from anonymous users&lt;/li&gt;&#xA;&lt;li&gt;&lt;a href=&quot;https://meta.stackexchange.com/a/92006&quot;&gt;Remove abandoned questions&lt;/a&gt;&lt;/li&gt;&#xA;&lt;/ul&gt;&#xA;",
            Views = 15839,
            UpVotes = 25243,
            DownVotes = 217539,
            AccountId = -1
        };
        
        // Post
        var newUserAccountActionResult = (CreatedAtActionResult) await controller.Post(mockUserAccount);
        var id = (string) newUserAccountActionResult.RouteValues?["Id"]!;
        
        // Get
        var gotUserAccount =  await controller.Get(id);
        var gotId = gotUserAccount.Value?.Id;
        Assert.Equal(id, gotId);
        
        // Delete (in fact, from the mongo db)
        await controller.Delete(id);
        // and delete from the redis
        await redisDb.KeyDeleteAsync(id);
        
        // try to get the user acc again
        var gotResult = await controller.Get(id);

        var resStr = gotResult.Result is null ? "yes" : "no";
        _testOutputHelper.WriteLine("gotResult: " + resStr);
        Assert.IsType<NotFoundResult>(gotResult.Result);
    }
}