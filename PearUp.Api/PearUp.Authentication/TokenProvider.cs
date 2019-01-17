using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PearUp.CommonEntities;
using PearUp.CommonEntity;
using PearUp.Constants;
using System;
using System.Text;

namespace PearUp.Authentication
{
    public class TokenProvider : ITokenProvider
    {
        private readonly int _expiryInMinutes;
        private readonly string _secretKey;
        private readonly string _cleintName;

        public TokenProvider(IOptions<AuthSettings> settings)
        {
            this._expiryInMinutes = settings.Value.ExpiryInMinutes;
            this._secretKey = settings.Value.SecretKey;
            this._cleintName = settings.Value.CleintName;
        }
        public Result<IAuthToken> CreateToken(TokenInfo tokenBuilderInput,bool isAdmin)
        {
            if (tokenBuilderInput == null)
                return Result.Fail<IAuthToken>(UserErrorMessages.User_Token_Object_Should_Not_Be_null);
            string policy = isAdmin ? AuthConstants.AdminId : AuthConstants.UserId;
            var token = new TokenBuilder().AddSecurityKey(GenerateSecret(_secretKey))
                            .AddSubject(tokenBuilderInput.FullName)
                            .AddIssuer(_cleintName)
                            .AddAudience(_cleintName)
                            .AddClaim(policy, Convert.ToString(tokenBuilderInput.Id))
                            .AddExpiry(_expiryInMinutes)
                            .Build();

            return Result.Ok((IAuthToken)token);
        }

        public static SymmetricSecurityKey GenerateSecret(string secret)
        {

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
