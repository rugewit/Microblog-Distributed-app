using System.Xml.Serialization;
namespace DataLoader.Models;
using Newtonsoft.Json;

[Serializable]
public class UserAccount
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [XmlAttribute("Id")]
    [JsonProperty("xmlId")]
    public int XmlId { get; set; }

    [XmlAttribute("Reputation")]
    [JsonProperty("reputation")]
    public int Reputation { get; set; }
   
    [XmlAttribute("CreationDate")]
    [JsonProperty("creationDate")]
    public DateTime CreationDate { get; set; }
   
    [XmlAttribute("DisplayName")]
    [JsonProperty("displayName")]
    public string DisplayName { get; set; } = null!;
   
    [XmlAttribute("LastAccessDate")]
    [JsonProperty("lastAccessDate")]
    public DateTime LastAccessDate { get; set; }
   
    [XmlAttribute("WebsiteUrl")]
    [JsonProperty("websiteUrl")]
    public string WebsiteUrl { get; set; } = null!;
   
    [XmlAttribute("Location")]
    [JsonProperty("location")]
    public string Location { get; set; } = null!;
   
    [XmlAttribute("AboutMe")]
    [JsonProperty("aboutMe")]
    public string AboutMe { get; set; } = null!;
   
    [XmlAttribute("Views")]
    [JsonProperty("views")]
    public int Views { get; set; }
   
    [XmlAttribute("UpVotes")]
    [JsonProperty("upVotes")]
    public int UpVotes { get; set; }
   
    [XmlAttribute("DownVotes")]
    [JsonProperty("downVotes")]
    public int DownVotes { get; set; }
   
    [XmlAttribute("AccountId")]
    [JsonProperty("accountId")]
    public int AccountId { get; set; }
}

[Serializable]
[XmlRoot("UserCollection")]
public class UserCollection
{
    [XmlArray("users")]
    [XmlArrayItem("row", typeof(UserAccount))]
    public UserAccount[]? UserAccounts { get; set; }
}