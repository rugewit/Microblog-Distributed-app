using System.Xml;
using MicroBlog.Models;
using MicroBlog.Services;

namespace MicroBlog.DataHandling;

public static class DatasetLoader
{
    private static string _datasetFolder = "/home/rugewit/MAI/nosql_dataset/askubuntu";
    private static string _usersDataset = Path.Combine(_datasetFolder, "Users.xml");
    private static string _postsDataset = Path.Combine(_datasetFolder, "Posts.xml");

    public static async Task LoadUsers(UserAccountsService userAccountsService)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var pathToFile = _usersDataset;
        var document = new XmlDocument();
        document.Load(pathToFile);
        var documentRoot = document.DocumentElement;
        var newUserAccounts = new List<UserAccount>();
        var counter = 0;
        if (documentRoot != null)
        {
            foreach (XmlElement node in documentRoot)
            {
                var attrColl = node.Attributes;
                var newUserAccount = new UserAccount();
                for (var i = 0; i < attrColl.Count; i++)
                {
                    var attrName = attrColl[i].Name;
                    var attrValue = attrColl[i].Value;
                    switch (attrName)
                    {
                        case "Reputation":
                            newUserAccount.Reputation = int.Parse(attrValue);
                            break;
                        case "CreationDate":
                            newUserAccount.CreationDate = attrValue;
                            break;
                        case "DisplayName":
                            newUserAccount.DisplayName = attrValue;
                            break;
                        case "LastAccessDate":
                            newUserAccount.LastAccessDate = attrValue;
                            break;
                        case "WebsiteUrl":
                            newUserAccount.WebsiteUrl = attrValue;
                            break;
                        case "Location":
                            newUserAccount.Location = attrValue;
                            break;
                        case "AboutMe":
                            newUserAccount.AboutMe = attrValue;
                            break;
                        case "Views":
                            newUserAccount.Views = int.Parse(attrValue);
                            break;
                        case "UpVotes":
                            newUserAccount.UpVotes = int.Parse(attrValue);
                            break;
                        case "DownVotes":
                            newUserAccount.DownVotes = int.Parse(attrValue);
                            break;
                        case "AccountId":
                            newUserAccount.AccountId = int.Parse(attrValue);
                            break;
                    }
                }
                newUserAccounts.Add(newUserAccount);
                counter++;
                if (counter % 100_000 == 0)
                {
                    Console.WriteLine($"Counter is {counter}");
                    Console.WriteLine($"Last user name is is {newUserAccounts.Last().DisplayName}");
                }
            }
            await userAccountsService.CreateManyAsync(newUserAccounts);
        }
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds / 1000.0;
        Console.WriteLine($"Elapsed time is {elapsed} seconds");
    }
    
     public static async Task LoadMessages(UserAccountsService userAccountsService)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var pathToFile = _usersDataset;
        var document = new XmlDocument();
        document.Load(pathToFile);
        var documentRoot = document.DocumentElement;
        var newUserAccounts = new List<UserAccount>();
        var counter = 0;
        if (documentRoot != null)
        {
            foreach (XmlElement node in documentRoot)
            {
                var attrColl = node.Attributes;
                var newUserAccount = new UserAccount();
                for (var i = 0; i < attrColl.Count; i++)
                {
                    var attrName = attrColl[i].Name;
                    var attrValue = attrColl[i].Value;
                    switch (attrName)
                    {
                        case "Reputation":
                            newUserAccount.Reputation = int.Parse(attrValue);
                            break;
                        case "CreationDate":
                            newUserAccount.CreationDate = attrValue;
                            break;
                        case "DisplayName":
                            newUserAccount.DisplayName = attrValue;
                            break;
                        case "LastAccessDate":
                            newUserAccount.LastAccessDate = attrValue;
                            break;
                        case "WebsiteUrl":
                            newUserAccount.WebsiteUrl = attrValue;
                            break;
                        case "Location":
                            newUserAccount.Location = attrValue;
                            break;
                        case "AboutMe":
                            newUserAccount.AboutMe = attrValue;
                            break;
                        case "Views":
                            newUserAccount.Views = int.Parse(attrValue);
                            break;
                        case "UpVotes":
                            newUserAccount.UpVotes = int.Parse(attrValue);
                            break;
                        case "DownVotes":
                            newUserAccount.DownVotes = int.Parse(attrValue);
                            break;
                        case "AccountId":
                            newUserAccount.AccountId = int.Parse(attrValue);
                            break;
                    }
                }
                newUserAccounts.Add(newUserAccount);
                counter++;
                if (counter % 100_000 == 0)
                {
                    Console.WriteLine($"Counter is {counter}");
                    Console.WriteLine($"Last user name is is {newUserAccounts.Last().DisplayName}");
                }
            }
            await userAccountsService.CreateManyAsync(newUserAccounts);
        }
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds / 1000.0;
        Console.WriteLine($"Elapsed time is {elapsed} seconds");
    }
}