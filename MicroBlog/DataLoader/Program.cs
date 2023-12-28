using System.Net.Http.Json;
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

static Message ModifyMessage(Message message)
{
    // Get the type of the Message class
    Type messageType = typeof(Message);

    // Get all string properties of the Message class
    var stringProperties = messageType.GetProperties()
        .Where(p => p.PropertyType == typeof(string));

    // Iterate through each string property and replace 'a' with 'b'
    foreach (var property in stringProperties)
    {
        string originalValue = (string) property.GetValue(message);
        if (originalValue != null)
        {
            // Replace 'a' with 'b'
            string modifiedValue = originalValue.Replace("\"", "");
            // Set the modified value back to the property
            property.SetValue(message, modifiedValue);
        }
    }

    return message;
}

static UserAccount ModifyUserAccount(UserAccount userAccount)
{
    // Get the type of the Message class
    Type userType = typeof(UserAccount);

    // Get all string properties of the Message class
    var stringProperties = userType.GetProperties()
        .Where(p => p.PropertyType == typeof(string));

    // Iterate through each string property and replace 'a' with 'b'
    foreach (var property in stringProperties)
    {
        string originalValue = (string) property.GetValue(userAccount);
        if (originalValue != null)
        {
            // Replace 'a' with 'b'
            string modifiedValue = originalValue.Replace("\"", "");
            // Set the modified value back to the property
            property.SetValue(userAccount, modifiedValue);
        }
    }

    return userAccount;
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

    var users = LoadUsers()[..usersSize].Select(UserRemoveQuotes).ToList();
    
    var usersJson = JsonConvert.SerializeObject(users);

    var usersContent = new StringContent(usersJson, Encoding.UTF8, "application/json");
    
    using var usersResponse = await httpClient.PostAsync("http://localhost:81/api/useraccounts/multiple", 
        usersContent);

    var content = await usersResponse.Content.ReadAsStringAsync();

    Console.WriteLine(content);
}

bool postMessages = true;

if (postMessages)
{
    // POST MESSAGES
    var messagesSize = 1000;

    var messages = LoadMessages()[..messagesSize].Select(MessageRemoveQuotes).ToList();
    
    var messagesJson = JsonConvert.SerializeObject(messages);
    
    var messagesContent = new StringContent(messagesJson, Encoding.UTF8, "application/json");
    
    using var messagesResponse = await httpClient.PostAsync("http://localhost:81/api/messages/multiple", 
        messagesContent);

    var content = await messagesResponse.Content.ReadAsStringAsync();

    Console.WriteLine(content);
}






