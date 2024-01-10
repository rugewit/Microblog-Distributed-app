namespace MicroBlog.Models.Settings;

public class ElasticSearchSettings
{
    public IEnumerable<string> ConnectionString { get; set; } = null!;
    public string IndexName { get; set; } = null!;
}