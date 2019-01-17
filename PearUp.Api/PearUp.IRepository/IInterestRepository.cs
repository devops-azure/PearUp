using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PearUp.IRepository
{
    public interface IInterestRepository : IBaseRepository<Interest>
    {
        Task<Result<IReadOnlyList<Interest>>> GetAllInterests();
        Task<Result<IReadOnlyList<Interest>>> GetInterestsByIds(int[] ids);
        Task<Result<Interest>> GetInterestById(int id);
    }
}
