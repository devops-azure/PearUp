using Microsoft.EntityFrameworkCore;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PearUp.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(PearUpContext pearUpContext) : base(pearUpContext)
        {

        }

        public async Task<Result<User>> GetUserByPhoneNumber(string phoneNumber)
        {
            return await Single(x => x.PhoneNumber.PhoneNumber == phoneNumber, UserErrorMessages.User_Does_Not_Exist);
        }
        
        public async Task<Result<User>> GetUserByIdAsync(int userId)
        {
            return await Single(x => x.Id == userId, x => x.Interests, UserErrorMessages.User_Does_Not_Exist);
        }
    }
}
