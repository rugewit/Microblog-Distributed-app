using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroBlog.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int PostTypeId { get; set; }
    public int AcceptedAnswerId { get; set; }
    public string CreationDate { get; set; } = null!;
    public int Score { get; set; }
    public int ViewCount { get; set; }
    public string Body { get; set; } = null!;
    public int OwnerUserId { get; set; }
    public int LastEditorUserId { get; set; }
    public string LastEditDate { get; set; } = null!;
    public string LastActivityDate { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Tags { get; set; } = null!;
    public int AnswerCount { get; set; }
    public int CommentCount { get; set; }
    public string ContentLicense { get; set; } = null!;
}