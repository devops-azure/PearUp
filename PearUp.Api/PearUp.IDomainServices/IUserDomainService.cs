using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using System.Threading.Tasks;

namespace PearUp.IDomainServices
{
    public interface IUserDomainService
    {
        Task<Result> SetUserInterests(User user, int[] interestIds);
    }
}
