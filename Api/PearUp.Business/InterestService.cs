using AutoMapper;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.DTO;
using PearUp.IApplicationServices;
using PearUp.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PearUp.ApplicationServices
{
    public class InterestService : IInterestService
    {
        private readonly IInterestRepository _interestRepository;
        private readonly IMapper _mapper;

        public InterestService(IInterestRepository interestRepository, IMapper mapper)
        {
            _interestRepository = interestRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<InterestDTO>>> GetInterests()
        {
            var interests = await _interestRepository.GetAllInterests();
            if (!interests.IsSuccessed)
                return Result.Fail<IReadOnlyList<InterestDTO>>(interests.GetErrorString());
            var list = _mapper.Map<IReadOnlyList<InterestDTO>>(interests.Value);
            return Result.Ok(list);
        }

        public async Task<Result> InsertInterest(CreateInterestDTO createInterestDTO)
        {
            if (createInterestDTO == null)
                return Result.Fail(Constants.InterestErrorMessages.Interest_Should_Not_Be_Empty);
            var interestResult = Interest.Create(createInterestDTO.InterestName, createInterestDTO.InterestDescription);
            if (!interestResult.IsSuccessed)
                return Result.Fail(interestResult.GetErrorString());

            _interestRepository.Create(interestResult.Value);
            return await _interestRepository.SaveChangesAsync(InterestErrorMessages.Error_Occured_While_Inserting_Interest);
        }

        public async Task<Result> UpdateInterest(InterestDTO interestDTO)
        {
            if (interestDTO == null)
                return Result.Fail(Constants.InterestErrorMessages.Interest_Should_Not_Be_Empty);
            var interest = Interest.Create(interestDTO.InterestName, interestDTO.InterestDescription);
            if (!interest.IsSuccessed)
                return Result.Fail(interest.GetErrorString());
            var actualInterest = await _interestRepository.GetInterestById(interestDTO.Id);
            if (!actualInterest.IsSuccessed)
                return Result.Fail(actualInterest.GetErrorString());
            actualInterest.Value.UpdateInterest(interest.Value);
            _interestRepository.Update(actualInterest.Value);
            return await _interestRepository.SaveChangesAsync(InterestErrorMessages.Error_Occured_While_Updating_Interest);
        }

    }
}
