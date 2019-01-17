using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PearUp.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<Result<User>> GetUserByPhoneNumber(string phoneNumber);
        Task<Result<User>> GetUserByIdAsync(int userId);
    }
}
