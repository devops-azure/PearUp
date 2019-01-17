using Microsoft.EntityFrameworkCore;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PearUp.Repository
{
    public class InterestRepository : BaseRepository<Interest>, IInterestRepository
    {
        public const string No_Interests_Found = "No interests found";

        public InterestRepository(PearUpContext context) : base(context)
        {
        }

        public async Task<Result<IReadOnlyList<Interest>>> GetAllInterests()
        {
            var interests = await _dbContext.Interests.AsNoTracking().ToListAsync();
            return ReturnInterestResult(interests);
        }

        public async Task<Result<IReadOnlyList<Interest>>> GetInterestsByIds(int[] ids)
        {
            var interests = await _dbContext.Interests.Where(interest => ids.Contains(interest.Id)).AsNoTracking().ToListAsync();
            return ReturnInterestResult(interests);
        }

        public async Task<Result<Interest>> GetInterestById(int id)
        {
            return await Single(x => x.Id == id, No_Interests_Found);
        }

        private Result<IReadOnlyList<Interest>> ReturnInterestResult(List<Interest> interests)
        {
            if (interests == null || !interests.Any())
            {
                return Result.Fail<IReadOnlyList<Interest>>(No_Interests_Found);
            }
            return Result.Ok<IReadOnlyList<Interest>>(interests);
        }
    }
}
