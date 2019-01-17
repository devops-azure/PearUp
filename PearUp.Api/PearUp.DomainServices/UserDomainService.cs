using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.IDomainServices;
using PearUp.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PearUp.DomainServices
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IInterestRepository _interestRepository;
        public const string Invalid_Interest = "Invalid Interest";

        public UserDomainService(IInterestRepository interestRepository)
        {
            _interestRepository = interestRepository;
        }

        public async Task<Result> SetUserInterests(User user, int[] interestIds)
        {
            if (interestIds == null || interestIds.Length <= 0)
            {
                return Result.Ok();
            }

            var interestsResult = await _interestRepository.GetInterestsByIds(interestIds);
            if (!interestsResult.IsSuccessed)
                return Result.Fail(interestsResult.GetErrorString());
            var interests = interestsResult.Value;
            
            if (AreInvalidInterestIdsPresent(interestIds, interests))
                return Result.Fail(Invalid_Interest);

            user.SetInterests(interests);

            return Result.Ok();
        }

        private bool AreInvalidInterestIdsPresent(int[] interestIds,IReadOnlyList<Interest> interests)
        {
            var uniqueIds = interestIds.Distinct().ToList();
            return uniqueIds.Count != interests.Count;
        }
    }
}
