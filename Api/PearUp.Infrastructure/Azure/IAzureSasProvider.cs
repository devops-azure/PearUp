using PearUp.CommonEntities;
using System.Threading.Tasks;

namespace PearUp.Infrastructure.Azure
{
    public interface IAzureSasProvider
    {
        /// <summary>
        /// Will create policy on top of container, and return saskey 
        /// </summary>
        /// <returns></returns>
        Task<AzureSasKey> GetContainerSAS();
    }
}
