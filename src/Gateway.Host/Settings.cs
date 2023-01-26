#nullable disable
namespace Gateway.Host;

public class Settings
{
    public string BlobConnectionString { get; set; }
    public string ServiceBlobContainerName { get; set; }
    public int ServiceCacheExpiryInSeconds { get; set; }
    public string JwtSecret { get; set; }
}