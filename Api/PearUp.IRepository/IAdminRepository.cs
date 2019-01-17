using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using System.Threading.Tasks;

namespace PearUp.IRepository
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
        Task<Result<Admin>> GetAdminByEmailId(string email);
    }
}
