using System;
using System.IdentityModel.Tokens.Jwt;

namespace PearUp.Authentication
{
    public sealed class AuthToken : IAuthToken
    {
        private JwtSecurityToken token;

        public AuthToken(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime ValidTo => token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);
    }
}
