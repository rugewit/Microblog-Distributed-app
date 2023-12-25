using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroBlog.Models;


[Serializable]
public class UserAccount
{
   [BsonId]
   [BsonRepresentation(BsonType.ObjectId)]
   public string? Id { get; set; }

   [XmlAttribute("Id")]
   public int XmlId { get; set; }

   [XmlAttribute("Reputation")]
   public int Reputation { get; set; }
   
   [XmlAttribute("CreationDate")]
   public DateTime CreationDate { get; set; }
   
   [XmlAttribute("DisplayName")]
   public string DisplayName { get; set; } = null!;
   
   [XmlAttribute("LastAccessDate")]
   public DateTime LastAccessDate { get; set; }
   
   [XmlAttribute("WebsiteUrl")]
   public string WebsiteUrl { get; set; } = null!;
   
   [XmlAttribute("Location")]
   public string Location { get; set; } = null!;
   
   [XmlAttribute("AboutMe")]
   public string AboutMe { get; set; } = null!;
   
   [XmlAttribute("Views")]
   public int Views { get; set; }
   
   [XmlAttribute("UpVotes")]
   public int UpVotes { get; set; }
   
   [XmlAttribute("DownVotes")]
   public int DownVotes { get; set; }
   
   [XmlAttribute("AccountId")]
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