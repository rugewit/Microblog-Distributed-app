using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DataLoader.Models;

[Serializable]
public class Message
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [XmlAttribute("Id")]
    [JsonProperty("xmlId")]
    public int XmlId { get; set; }

    [XmlAttribute("PostTypeId")]
    [JsonProperty("postTypeId")]
    public int PostTypeId { get; set; }
    
    [XmlAttribute("AcceptedAnswerId")]
    [JsonProperty("acceptedAnswerId")]
    public int AcceptedAnswerId { get; set; }
    
    [XmlAttribute("CreationDate")]
    [JsonProperty("creationDate")]
    public DateTime CreationDate { get; set; }
    
    [XmlAttribute("Score")]
    [JsonProperty("score")]
    public int Score { get; set; }
    
    [XmlAttribute("ViewCount")]
    [JsonProperty("viewCount")]
    public int ViewCount { get; set; }
    
    [XmlAttribute("Body")]
    [JsonProperty("body")]
    public string Body { get; set; } = null!;
    
    [XmlAttribute("OwnerUserId")]
    [JsonProperty("ownerUserId")]
    public int OwnerUserId { get; set; }
    
    [XmlAttribute("LastEditorUserId")]
    [JsonProperty("lastEditorUserId")]
    public int LastEditorUserId { get; set; }
    
    [XmlAttribute("LastEditDate")]
    [JsonProperty("lastEditDate")]
    public DateTime LastEditDate { get; set; }
    
    [XmlAttribute("LastActivityDate")]
    [JsonProperty("lastActivityDate")]
    public DateTime LastActivityDate { get; set; }
    
    [XmlAttribute("Title")]
    [JsonProperty("title")]
    public string Title { get; set; } = null!;
    
    [XmlAttribute("Tags")]
    [JsonProperty("tags")]
    public string Tags { get; set; } = null!;
    
    [XmlAttribute("AnswerCount")]
    [JsonProperty("answerCount")]
    public int AnswerCount { get; set; }
    
    [XmlAttribute("CommentCount")]
    [JsonProperty("commentCount")]
    public int CommentCount { get; set; }
    
    [XmlAttribute("ContentLicense")]
    [JsonProperty("contentLicense")]
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