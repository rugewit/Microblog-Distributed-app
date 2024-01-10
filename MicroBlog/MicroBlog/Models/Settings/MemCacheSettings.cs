namespace MicroBlog.Models.Settings;

public class MemCacheSettings
{
    public IEnumerable<string> Addresses { get; set; } = null!;
    public IEnumerable<int> Ports { get; set; } = null!;
}