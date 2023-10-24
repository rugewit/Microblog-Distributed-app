namespace MicroBlog.Models.Settings;

public class UserAccountsExpirePolicySettings
{
    public int UserAccountCacheExpireTimeSec { get; set; }
    public int RedactionTimeSimulationSec { get; set; }
}