using PearUp.CommonEntities;
using System.Collections.Generic;

namespace PearUp.BusinessEntity
{
    public class Password : ValueObject
    {
        public const string Password_Hash_Is_Required = "Password hash is required.";
        public const string Password_Salt_Is_Required = "Password salt is required.";

        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        
        private Password()
        {

        }

        private Password(byte[] passwordHash, byte[] passwordSalt)
        {
            this.PasswordHash = passwordHash;
            this.PasswordSalt = passwordSalt;
        }

        public static Result<Password> Create(byte[] passwordHash, byte[] passwordSalt)
        {
            if (passwordHash == null)
                return Result.Fail<Password>(Password_Hash_Is_Required);
            if (passwordSalt== null)
                return Result.Fail<Password>(Password_Salt_Is_Required);
            return Result.Ok(new Password(passwordHash, passwordSalt));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PasswordHash;
            yield return PasswordSalt;
        }
    }
}