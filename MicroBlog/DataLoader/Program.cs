using System.Net.Http.Json;
using System.Xml.Serialization;
using MicroBlog.Models;
using MicroBlog.Utils;
using Newtonsoft.Json;

const string UsersPath = "/home/rugewit/MAI/nosql_dataset/askubuntu/Users.xml";
const string PostsPath = "/home/rugewit/MAI/nosql_dataset/askubuntu/Posts.xml";

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

var httpClient = new HttpClient();

bool checkAvailability = true;

if (checkAvailability)
{
    // check availability
    var address = "http://localhost:81/";

    using var response = await httpClient.GetAsync(address);

    string content = await response.Content.ReadAsStringAsync();

    Console.WriteLine(content);
}

bool postUsers = true;

if (postUsers)
{
    // POST USERS
    var usersSize = 1000;

    var users = LoadUsers()[..usersSize].ToList();

    var usersContent = JsonContent.Create(users[0]);

    using var usersResponse = await httpClient.PostAsync("http://localhost:81/api/useraccounts/", 
        usersContent);

    var content = await usersResponse.Content.ReadAsStringAsync();

    Console.WriteLine(content);
}






