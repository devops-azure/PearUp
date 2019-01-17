using Microsoft.AspNetCore.Http;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.CommonEntity;
using PearUp.Constants;
using PearUp.IRepository;
using System;
using System.Threading.Tasks;

namespace PearUp.Authentication
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public IdentityProvider(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userRepository = userRepository;
        }

        /// <summary>
        /// Get Current logged in user
        /// </summary>
        /// <returns></returns>
        public Result<CurrentUser> GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.HasClaim(c => c.Type == AuthConstants.UserId && int.Parse(c.Value) > default(int)))
                return Result.Fail<CurrentUser>(CommonErrorMessages.User_Not_Authenticated);

            var userId = Convert.ToInt32(user.FindFirst(c => c.Type == AuthConstants.UserId).Value);
            return Result.Ok(CurrentUser.Create(userId));
        }

        public async Task<Result<User>> GetUser()
        {
            var currentUserResult = GetCurrentUser();

            if (!currentUserResult.IsSuccessed)
                return Result.Fail<User>(currentUserResult.GetErrorString());

            var userResult = await _userRepository.GetUserByIdAsync(currentUserResult.Value.UserId);

            if (!userResult.IsSuccessed)
                return Result.Fail<User>(userResult.GetErrorString());

            return Result.Ok(userResult.Value);
        }
    }
}
