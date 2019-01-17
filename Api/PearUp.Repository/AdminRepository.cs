using System.Threading.Tasks;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IRepository;

namespace PearUp.Repository
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public const string Admin_Not_Present_With_Given_Email = "Admin not present in system with given email";
        public AdminRepository(PearUpContext context) : base(context)
        {
        }

        public async Task<Result<Admin>> GetAdminByEmailId(string email)
        {
            return await Single(x => x.Email.EmailId == email, Admin_Not_Present_With_Given_Email);
        }
    }
}
