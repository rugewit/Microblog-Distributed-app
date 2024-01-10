using System.Text;
using System.Xml.Serialization;
using DataLoader.Models;
using Newtonsoft.Json;

const string UsersPath = "/home/rugewit/MAI/nosql_dataset/askubuntu/Users.xml";
const string PostsPath = "/home/rugewit/MAI/nosql_dataset/askubuntu/Posts.xml";

static Message MessageRemoveQuotes(Message message)
{
    message.Id = "";
    if (message.Body == null) message.Body = "";
    if (message.Title == null) message.Title = "";
    if (message.Tags == null) message.Tags = "";
    if (message.ContentLicense == null) message.ContentLicense = "";
    
    message.Body = message.Body.Replace("\"", "");
    message.Title = message.Title.Replace("\"", "");
    message.Tags = message.Tags.Replace("\"", "");
    message.ContentLicense = message.ContentLicense.Replace("\"", "");
    
    return message;
}

static UserAccount UserRemoveQuotes(UserAccount user)
{
    user.Id = "";
    if (user.DisplayName == null) user.DisplayName = "";
    if (user.WebsiteUrl == null) user.WebsiteUrl = "";
    if (user.Location == null) user.Location = "";
    if (user.AboutMe == null) user.AboutMe = "";
    
    user.DisplayName = user.DisplayName.Replace("\"", "");
    user.WebsiteUrl = user.WebsiteUrl.Replace("\"", "");
    user.Location = user.Location.Replace("\"", "");
    user.AboutMe = user.AboutMe.Replace("\"", "");

    return user;
}

static UserAccount[] LoadUsers()
{
    var watch = System.Diagnostics.Stopwatch.StartNew();
    var pathToFile = UsersPath;
    
    UserCollection? newUserAccounts = null;
    
    var serializer = new XmlSerializer(typeof(UserCollection));

    var reader = new StreamReader(pathToFile);
    newUserAccounts = (UserCollection) serializer.Deserialize(reader)!;
    reader.Close();
    var elapsed = watch.ElapsedMilliseconds / 1000.0;
    Console.WriteLine($"LoadUsers() Elapsed time for deserialization is {elapsed} seconds");
    watch.Stop();

    return newUserAccounts.UserAccounts;
}
    
    
static Message[] LoadMessages()
{
    var watch = System.Diagnostics.Stopwatch.StartNew();
    var pathToFile = PostsPath;
    
    MessageCollection? messageCollection = null;
    
    // Deserialize
    var serializer = new XmlSerializer(typeof(MessageCollection));
    
    var reader = new StreamReader(pathToFile);
    messageCollection = (MessageCollection) serializer.Deserialize(reader)!;
    reader.Close();
    var elapsed = watch.ElapsedMilliseconds / 1000.0;
    Console.WriteLine($"LoadMessages() Elapsed time for deserialization is {elapsed} seconds");
    watch.Stop();

    return messageCollection.Messages;
}


static async Task<string> PostDataAsync<T>(IEnumerable<T> curBatch, HttpClient httpClient, string url)
{
    var dataJson = JsonConvert.SerializeObject(curBatch);
    var dataContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
    using var response = await httpClient.PostAsync(url, dataContent);
    
    var content = await response.Content.ReadAsStringAsync();
    return content;
}


var httpClient = new HttpClient();

bool checkAvailability = true;

if (checkAvailability)
{
    // check availability
    var address = "http://localhost:5002/";

    using var response = await httpClient.GetAsync(address);

    string content = await response.Content.ReadAsStringAsync();

    Console.WriteLine(content);
}

var usersSize = 30_000;
var messagesSize = 30_000;

var batchSize = 2000;

var postUsers = true;
var postMessages = true;

if (postUsers)
{
    // POST USERS
    const string usersUrl = "http://localhost:5002/api/useraccounts/multiple";

    var users = LoadUsers()[..usersSize].Select(UserRemoveQuotes).ToList();
    
    if (usersSize < batchSize)
    {
        var res = await PostDataAsync(users, httpClient, usersUrl);
        Console.WriteLine(res);
    }
    else
    {
        var curBatch = new List<UserAccount>();
        for (var i = 1; i <= users.Count; i++)
        {
            curBatch.Add(users[i-1]);
            if (i % batchSize == 0)
            {
                var res = await PostDataAsync(curBatch, httpClient, usersUrl);
                Console.WriteLine(res);
                curBatch.Clear();
                Console.WriteLine($"***********************\ni={i} users is already inserted");
            }
        }

        if (curBatch.Count != 0)
        {
            var res = await PostDataAsync(curBatch, httpClient, usersUrl);
            Console.WriteLine(res);
            curBatch.Clear();
        }
    }
}


if (postMessages)
{
    // POST MESSAGES
    const string messagesUrl = "http://localhost:5002/api/messages/multiple";

    var messages = LoadMessages()[..messagesSize].Select(MessageRemoveQuotes).ToList();
    
    if (messagesSize < batchSize)
    {
        var res = await PostDataAsync(messages, httpClient, messagesUrl);
        Console.WriteLine(res);
    }
    else
    {
        var curBatch = new List<Message>();
        for (var i = 1; i <= messages.Count; i++)
        {
            curBatch.Add(messages[i-1]);
            if (i % batchSize == 0)
            {
                var res = await PostDataAsync(curBatch, httpClient, messagesUrl);
                Console.WriteLine(res);
                curBatch.Clear();
                Console.WriteLine($"***********************\ni={i} messages is already inserted");
            }
        }

        if (curBatch.Count != 0)
        {
            var res = await PostDataAsync(curBatch, httpClient, messagesUrl);
            Console.WriteLine(res);
            curBatch.Clear();
        }
    }
}






