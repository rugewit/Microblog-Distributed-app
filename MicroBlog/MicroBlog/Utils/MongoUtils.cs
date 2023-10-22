using MongoDB.Bson;

namespace MicroBlog.Utils;

public static class MongoUtils
{
    public static bool IsValidMongoId(string id) =>
        ObjectId.TryParse(id, out _);
}