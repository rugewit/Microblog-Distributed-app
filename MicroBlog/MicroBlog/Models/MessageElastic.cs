namespace MicroBlog.Models;

public class MessageElastic
{
    public string? BsonId { get; set; }
    
    public int XmlId { get; set; }

    public int PostTypeId { get; set; }

    public int AcceptedAnswerId { get; set; }

    public DateTime CreationDate { get; set; }

    public int Score { get; set; }

    public int ViewCount { get; set; }

    public string Body { get; set; } = null!;

    public int OwnerUserId { get; set; }

    public int LastEditorUserId { get; set; }

    public DateTime LastEditDate { get; set; }

    public DateTime LastActivityDate { get; set; }

    public string Title { get; set; } = null!;

    public string Tags { get; set; } = null!;

    public int AnswerCount { get; set; }

    public int CommentCount { get; set; }

    public string ContentLicense { get; set; } = null!;
}