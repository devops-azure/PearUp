using PearUp.BusinessEntity;
using System;
using System.Collections.Generic;

namespace PearUp.RepositoryEntity
{
    public class User : Audit
    {
        public User()
        {
            UserInterests = new HashSet<UserInterest>();
        }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CountryCode { get; set; }

        public bool IsAdmin { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public int LookingFor { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int Distance { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Profession { get; set; }
        public string School { get; set; }

        public string FunAndInterestingThings { get; set; }
        public string BucketList { get; set; }


        public ICollection<UserInterest> UserInterests { get; set; }
    }
}
