using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroBlog.Models;

[Serializable]
public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [XmlAttribute("Id")]
    public int XmlId { get; set; }

    [XmlAttribute("PostTypeId")]
    public int PostTypeId { get; set; }
    
    [XmlAttribute("AcceptedAnswerId")]
    public int AcceptedAnswerId { get; set; }
    
    [XmlAttribute("CreationDate")]
    public string CreationDate { get; set; } = null!;
    
    [XmlAttribute("Score")]
    public int Score { get; set; }
    
    [XmlAttribute("ViewCount")]
    public int ViewCount { get; set; }
    
    [XmlAttribute("Body")]
    public string Body { get; set; } = null!;
    
    [XmlAttribute("OwnerUserId")]
    public int OwnerUserId { get; set; }
    
    [XmlAttribute("LastEditorUserId")] 
    public int LastEditorUserId { get; set; }
    
    [XmlAttribute("LastEditDate")]
    public string LastEditDate { get; set; } = null!;
    
    [XmlAttribute("LastActivityDate")]
    public string LastActivityDate { get; set; } = null!;
    
    [XmlAttribute("Title")]
    public string Title { get; set; } = null!;
    
    [XmlAttribute("Tags")]
    public string Tags { get; set; } = null!;
    
    [XmlAttribute("AnswerCount")]
    public int AnswerCount { get; set; }
    
    [XmlAttribute("CommentCount")]
    public int CommentCount { get; set; }
    
    [XmlAttribute("ContentLicense")]
    public string ContentLicense { get; set; } = null!;
}

[Serializable]
[XmlRoot("MessageCollection")]
public class MessageCollection
{
    [XmlArray("posts")]
    [XmlArrayItem("row", typeof(Message))]
    public Message[]? Messages { get; set; }
}