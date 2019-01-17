using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.DTO.PhoneVerification
{
    public class ValidatePhoneVerificationDTO
    {
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public int VerificationCode { get; set; }
    }
}
