using System;
using System.ComponentModel.DataAnnotations;

namespace PearUp.DTO
{
    public class UserRegistrationDTO
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Profession { get; set; }
        public string School { get; set; }
        public int LookingFor { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int Distance { get; set; }
        public string FunAndInteresting { get; set; }
        public string BucketList { get; set; }
        public int[] InterestIds { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}