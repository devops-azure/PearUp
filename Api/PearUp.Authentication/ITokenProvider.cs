using Microsoft.IdentityModel.Tokens;
using PearUp.CommonEntities;
using PearUp.CommonEntity;

namespace PearUp.Authentication
{
    public interface ITokenProvider
    {
        Result<IAuthToken> CreateToken(TokenInfo userToken, bool isAdmin);
    }
}
