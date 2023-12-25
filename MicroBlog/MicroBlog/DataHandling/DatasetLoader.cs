using System.Xml.Serialization;
using MicroBlog.Controllers;
using MicroBlog.Models;
using MicroBlog.Models.Settings;
using MicroBlog.Services.Interfaces;
using MicroBlog.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroBlog.DataHandling;

public static class DatasetLoader
{
    public static async Task LoadUsersIntoDb(IUserAccountsService userAccountsService,
        IOptions<DatasetPathSettings> datasetPath, ILogger logger)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var pathToFile = datasetPath.Value.UserAccountsPath;
        
        UserCollection? newUserAccounts = null;
        
        var serializer = new XmlSerializer(typeof(UserCollection));

        var reader = new StreamReader(pathToFile);
        newUserAccounts = (UserCollection) serializer.Deserialize(reader)!;
        reader.Close();
        var elapsed = watch.ElapsedMilliseconds / 1000.0;
        //Console.WriteLine($"LoadUsers() Elapsed time for deserialization is {elapsed} seconds");
        logger.LogInformation($"LoadUsers() Elapsed time for deserialization is {elapsed} seconds");
        watch.Stop();
        
        watch = System.Diagnostics.Stopwatch.StartNew();
        await userAccountsService.CreateManyAsync(newUserAccounts.UserAccounts);
        
        elapsed = watch.ElapsedMilliseconds / 1000.0;
        //Console.WriteLine($"LoadUsers() Elapsed time for inserting into mongo db is {elapsed} seconds");
        logger.LogInformation($"LoadUsers() Elapsed time for inserting into mongo db is {elapsed} seconds");
        watch.Stop();
    }
    
    
     public static async Task LoadMessagesIntoDb(IMessagesService messagesService,
         IElasticSearchService elasticSearchService,
         IOptions<DatasetPathSettings> datasetPath, ILogger logger)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var pathToFile = datasetPath.Value.MessagesPath;
        
        MessageCollection? messageCollection = null;
        
        // Deserialize
        var serializer = new XmlSerializer(typeof(MessageCollection));
        
        var reader = new StreamReader(pathToFile);
        messageCollection = (MessageCollection) serializer.Deserialize(reader)!;
        reader.Close();
        var elapsed = watch.ElapsedMilliseconds / 1000.0;
        //Console.WriteLine($"LoadMessages() Elapsed time for deserialization is {elapsed} seconds");
        logger.LogInformation($"LoadMessages() Elapsed time for deserialization is {elapsed} seconds");
        watch.Stop();
        
        // Insert in mongo db
        watch = System.Diagnostics.Stopwatch.StartNew();
        await messagesService.CreateManyAsync(messageCollection.Messages);
        
        elapsed = watch.ElapsedMilliseconds / 1000.0;
        //Console.WriteLine($"LoadMessages() Elapsed time for inserting into mongo db is {elapsed} seconds");
        logger.LogInformation($"LoadMessages() Elapsed time for inserting into mongo db is {elapsed} seconds");
        watch.Stop();
    }

     public static async Task LoadMessagesIntoElastic(IMessagesService messagesService,
         IElasticSearchService elasticSearchService,
         IOptions<DatasetPathSettings> datasetPath, ILogger logger)
     {
         var watch = System.Diagnostics.Stopwatch.StartNew();
         var pathToFile = datasetPath.Value.MessagesPath;
        
         MessageCollection? messageCollection = null;
        
         // Deserialize
         var serializer = new XmlSerializer(typeof(MessageCollection));
        
         var reader = new StreamReader(pathToFile);
         messageCollection = (MessageCollection) serializer.Deserialize(reader)!;
         reader.Close();
         var elapsed = watch.ElapsedMilliseconds / 1000.0;
         //Console.WriteLine($"LoadMessages() Elapsed time for deserialization is {elapsed} seconds");
         logger.LogInformation($"LoadMessages() Elapsed time for deserialization is {elapsed} seconds");
         watch.Stop();
         
         // Insert in ElasticSearch
         watch = System.Diagnostics.Stopwatch.StartNew();

         var allDataForElastic = 
             messageCollection.Messages.Select(ModelsUtils.MessageToMessageElastic).ToList();

         const int batchSize = 20_000;
        
         //var elasticController = new ElasticSearchControllerTest(elasticSearchService);
        
         var curBatch = new List<MessageElastic>();
         for (var i = 1; i <= allDataForElastic.Count; i++)
         {
             curBatch.Add(allDataForElastic[i-1]);
             if (i % batchSize == 0)
             {
                 await elasticSearchService.CreateManyAsync(curBatch);
                 //await elasticController.PostMany(curBatch);
                 curBatch.Clear();
                 Console.WriteLine($"***********************\ni={i} is already inserted");
             }
         }

         if (curBatch.Count != 0)
         {
             await elasticSearchService.CreateManyAsync(curBatch);
             curBatch.Clear();
         }
        
         elapsed = watch.ElapsedMilliseconds / 1000.0;
         //Console.WriteLine($"LoadMessages() Elapsed time for inserting into mongo db is {elapsed} seconds");
         logger.LogInformation($"LoadMessages() Elapsed time for inserting into ElasticSearch is {elapsed} seconds");
         watch.Stop();
     }
     
}