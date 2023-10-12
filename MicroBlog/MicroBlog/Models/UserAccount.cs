using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroBlog.Models;

public class UserAccount
{
   [BsonId]
   [BsonRepresentation(BsonType.ObjectId)]
   public string? Id { get; set; }
   
   public int Reputation { get; set; }
   
   public string CreationDate { get; set; } = null!;
   
   public string DisplayName { get; set; } = null!;
   
   public string LastAccessDate { get; set; } = null!;
   
   public string WebsiteUrl { get; set; } = null!;
   
   public string Location { get; set; } = null!;
   
   public string AboutMe { get; set; } = null!;
   
   public int Views { get; set; }
   
   public int UpVotes { get; set; }
   
   public int DownVotes { get; set; }
   
   public int AccountId { get; set; }
}