using MicroBlog.Models;

namespace MicroBlog.Utils;

public static class ModelsUtils
{
    public static MessageElastic MessageToMessageElastic(Message message)
    {
        return new MessageElastic
        {
            BsonId = message.Id,
            XmlId = message.XmlId,
            PostTypeId = message.PostTypeId,
            AcceptedAnswerId = message.AcceptedAnswerId,
            CreationDate = message.CreationDate,
            Score = message.Score,
            ViewCount = message.ViewCount,
            Body = message.Body,
            OwnerUserId = message.OwnerUserId,
            LastEditorUserId = message.LastEditorUserId,
            LastEditDate = message.LastEditDate,
            LastActivityDate = message.LastActivityDate,
            Title = message.Title,
            Tags = message.Tags,
            AnswerCount = message.AnswerCount,
            CommentCount = message.CommentCount,
            ContentLicense = message.ContentLicense,
        };
    }
    
    public static Message MessageElasticToMessage(MessageElastic messageElastic)
    {
        return new Message
        {
            XmlId = messageElastic.XmlId,
            PostTypeId = messageElastic.PostTypeId,
            AcceptedAnswerId = messageElastic.AcceptedAnswerId,
            CreationDate = messageElastic.CreationDate,
            Score = messageElastic.Score,
            ViewCount = messageElastic.ViewCount,
            Body = messageElastic.Body,
            OwnerUserId = messageElastic.OwnerUserId,
            LastEditorUserId = messageElastic.LastEditorUserId,
            LastEditDate = messageElastic.LastEditDate,
            LastActivityDate = messageElastic.LastActivityDate,
            Title = messageElastic.Title,
            Tags = messageElastic.Tags,
            AnswerCount = messageElastic.AnswerCount,
            CommentCount = messageElastic.CommentCount,
            ContentLicense = messageElastic.ContentLicense,
        };
    }
}