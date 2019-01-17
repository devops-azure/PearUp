using PearUp.CommonEntities;
using PearUp.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PearUp.IApplicationServices
{
    public interface IInterestService
    {
        Task<Result<IReadOnlyList<InterestDTO>>> GetInterests();
        Task<Result> InsertInterest(CreateInterestDTO interestDTO);
        Task<Result> UpdateInterest(InterestDTO interestDTO);

    }
}
