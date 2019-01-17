using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace PearUp.Infrastructure.Azure
{
    public class ContainerFactory : IContainerFactory
    {
        private readonly string connectionString;
        private readonly string containerName;
        private CloudBlobContainer _container;
        private static readonly object padlock = new object();


        public ContainerFactory(IOptions<AzureSettings> options)
        {
            connectionString = options.Value.ConnectionString;
            containerName = options.Value.ContainerName;
        }

        public async Task<CloudBlobContainer> CreateContainerReference()
        {
            if (_container != null)
                return _container;
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
            await _container.CreateIfNotExistsAsync();
            return _container;
        }
    }
}
