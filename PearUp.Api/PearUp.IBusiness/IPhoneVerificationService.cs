using PearUp.CommonEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.IBusiness
{
    public interface IPhoneVerificationService
    {
        Task<Result> GenerateVerificationCodeAsync(string countryCode, string phoneNumber);
        Task<Result> ValidateVerificationCodeAsync(string countryCode, string phoneNumber, int verificationCode);
    }
}
