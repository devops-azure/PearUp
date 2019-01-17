using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using System.Threading.Tasks;

namespace PearUp.IBusiness
{
    public interface IPearUpAuthenticationService
    {
        Task<Result<User>> Authenticate(string phoneNumber, string password);

        Task<Result<Admin>> AuthenticateAdmin(string email, string password);
    }
}
