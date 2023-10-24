namespace MicroBlog.Models.Settings;

public class MicroBlogDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UserAccountsCollectionName { get; set; } = null!;
    public string MessagesCollectionName { get; set; } = null!;
}