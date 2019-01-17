using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.DTO.PhoneVerification
{
    public class TwilioErrorResponse
    {
        public int error_code { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public Errors errors { get; set; }
    }

    public class Errors
    {
        public string message { get; set; }
    }
}
