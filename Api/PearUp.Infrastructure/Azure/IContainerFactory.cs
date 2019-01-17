using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace PearUp.Infrastructure.Azure
{
    public interface IContainerFactory
    {
        Task<CloudBlobContainer> CreateContainerReference();
    }
}
