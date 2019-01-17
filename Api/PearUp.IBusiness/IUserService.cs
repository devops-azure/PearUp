using PearUp.CommonEntities;
using PearUp.BusinessEntity;
using System.Threading.Tasks;
using System.Collections.Generic;
using PearUp.DTO;

namespace PearUp.IBusiness
{
    public interface IUserService
    {
        Task<Result<User>> GetUser(string phoneNumber);
        Task<Result> SetUserInterestsAsync(int[] interestIds);
        Task<Result<User>> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO);
        Task<Result> SetLocationAsync(UserLocationDTO userLocationDTO);
    }
}
