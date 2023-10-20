using System.Xml;
using System.Xml.Serialization;
using MicroBlog.Models;
using MicroBlog.Services;
using Microsoft.Extensions.Options;

namespace MicroBlog.DataHandling;

public static class DatasetLoader
{
    public static async Task LoadUsers(IUserAccountsService userAccountsService,
        IOptions<DatasetPathSettings> datasetPath)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var pathToFile = datasetPath.Value.UserAccountsPath;
        
        UserCollection? newUserAccounts = null;
        
        var serializer = new XmlSerializer(typeof(UserCollection));

        var reader = new StreamReader(pathToFile);
        newUserAccounts = (UserCollection) serializer.Deserialize(reader)!;
        reader.Close();
        var elapsed = watch.ElapsedMilliseconds / 1000.0;
        Console.WriteLine($"LoadUsers() Elapsed time for deserialization is {elapsed} seconds");
        watch.Stop();
        
        watch = System.Diagnostics.Stopwatch.StartNew();
        await userAccountsService.CreateManyAsync(newUserAccounts.UserAccounts);
        
        elapsed = watch.ElapsedMilliseconds / 1000.0;
        Console.WriteLine($"LoadUsers() Elapsed time for inserting into mongo db is {elapsed} seconds");
        watch.Stop();
    }
    
    
     public static async Task LoadMessages(IMessagesService messagesService, 
         IOptions<DatasetPathSettings> datasetPath)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var pathToFile = datasetPath.Value.MessagesPath;
        
        MessageCollection? messageCollection = null;
        
        var serializer = new XmlSerializer(typeof(MessageCollection));

        var reader = new StreamReader(pathToFile);
        messageCollection = (MessageCollection) serializer.Deserialize(reader)!;
        reader.Close();
        var elapsed = watch.ElapsedMilliseconds / 1000.0;
        Console.WriteLine($"LoadMessages() Elapsed time for deserialization is {elapsed} seconds");
        watch.Stop();
        
        watch = System.Diagnostics.Stopwatch.StartNew();
        await messagesService.CreateManyAsync(messageCollection.Messages);
        
        elapsed = watch.ElapsedMilliseconds / 1000.0;
        Console.WriteLine($"LoadMessages() Elapsed time for inserting into mongo db is {elapsed} seconds");
        watch.Stop();
    }
}