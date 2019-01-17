using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.Business
{
    public abstract class UserVerificationService
    {
        private readonly IUserRepository _userRepository;

        public UserVerificationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //protected async Task<Result> ChangeStatus(BusinessEntity.User user, UserStatus status)
        //{
        //    user.Status = status;
        //    _userRepository.Update(user);
        //    var result = await _userRepository.SaveChangesAsync("Failed to verify user");
        //    return result;
        //}
    }
}
