using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.Factories.Interfaces
{
    public interface IUserFactory
    {
        Task<Result<User>> CreateUserAsync(UserRegistrationDTO userRegistrationDTO);
    }
}
