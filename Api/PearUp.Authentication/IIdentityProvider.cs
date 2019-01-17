using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.CommonEntity;
using System.Threading.Tasks;

namespace PearUp.Authentication
{
    public interface IIdentityProvider
    {
        Result<CurrentUser> GetCurrentUser();

        Task<Result<User>> GetUser();
    }
}
