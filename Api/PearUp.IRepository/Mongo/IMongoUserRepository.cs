using PearUp.BusinessEntity;
using System.Threading.Tasks;

namespace PearUp.IRepository
{
    public interface IMongoUserRepository 
    {
        Task CreateAsync(User entity);
        Task ReplaceUserAsync(User user);

    }
}
