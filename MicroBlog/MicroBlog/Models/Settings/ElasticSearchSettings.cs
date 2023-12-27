namespace MicroBlog.Models.Settings;

public class ElasticSearchSettings
{
    public string ConnectionString { get; set; } = null!;
    //public string CertificateFingerprint { get; set; } = null!;
    //public string Username { get; set; } = null!;
    //public string Password { get; set; } = null!;
    public string IndexName { get; set; } = null!;
}