using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }

        public override bool Equals(object obj)
        {
            return this.Token == ((LoginResponseDTO)obj).Token;
        }

        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }
    }
}
