namespace PearUp.Infrastructure.Azure
{
    public class AzureSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public int ExpireTimeInMinutes { get; set; }
    }
}
