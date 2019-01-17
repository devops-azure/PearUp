using Microsoft.AspNetCore.Mvc;
using PearUp.Authentication;
using PearUp.BusinessEntity;
using PearUp.CommonEntity;

namespace PearUp.Api.Controllers
{
    public abstract class TokenController : Controller
    {
        private readonly ITokenProvider _tokenProvider;

        public TokenController(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
        /// <summary>
        /// create token for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected CommonEntities.Result<IAuthToken> CreateUserToken(User user)
        {
            var userToken =new TokenInfo
            {
                Id = user.Id,
                FullName = user.FullName,
            };
            return CreateToken(userToken,false);
        }

        /// <summary>
        /// create token for Admin
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        protected CommonEntities.Result<IAuthToken> CreateAdminToken(Admin admin)
        {
            var adminTokenBuilderInput =new TokenInfo
            {
                Id = admin.Id,
                FullName = admin.FullName,
            };
            return CreateToken(adminTokenBuilderInput,true);
            //token creating
        }
        private CommonEntities.Result<IAuthToken> CreateToken(TokenInfo tokenBuilderInput,bool isToken)
        {
            var token = _tokenProvider.CreateToken(tokenBuilderInput, isToken);
            if (!token.IsSuccessed)
                return CommonEntities.Result.Fail<IAuthToken>(token.GetErrorString());

            return token;
        }
    }
}
