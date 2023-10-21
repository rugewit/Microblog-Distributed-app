using MicroBlog.Controllers;
using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Tests;

public class MessagesRestTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MessagesRestTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void PostGetDelete()
    {
        var app = WebAppForTest.GetTestApp();

        var messagesService = app.Services.GetRequiredService<IMessagesService>();

        var controller = new MessagesController(messagesService);

        var mockMessage = new Message
        {
            XmlId = 322,
            PostTypeId =  322,
            AcceptedAnswerId = 322,
            CreationDate = "2010-07-28T19:04:21.300",
            Score = 62,
            ViewCount = 4484,
            Body =  "<p>Every time I turn on my computer, I see a message saying something …",
            OwnerUserId =  5,
            LastEditorUserId = 208574,
            LastEditDate = "2014-12-16T01:47:45.980",
            LastActivityDate = "2018-10-05T23:56:48.997",
            Title = "MockMessageMockMock",
            Tags = "<power-management><notification>",
            AnswerCount =  4,
            CommentCount =  2,
            ContentLicense = "CC BY-SA 3.0",
        };

        // Post
        var newMessageActionResult = (CreatedAtActionResult) await controller.Post(mockMessage);
        var id = (string) newMessageActionResult.RouteValues?["Id"]!;

        // Get
        var gotMessage = await controller.Get(id);
        var gotId = gotMessage.Value?.Id;
        Assert.Equal(id, gotId);

        // Delete
        await controller.Delete(id);

        // try to get the user acc again
        var gotResult = await controller.Get(id);
        Assert.IsType<NotFoundResult>(gotResult.Result);
    }
}