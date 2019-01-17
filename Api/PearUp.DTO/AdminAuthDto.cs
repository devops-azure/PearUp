using PearUp.Authentication;
using PearUp.BusinessEntity;

namespace PearUp.Dto
{
    public class AdminAuthDto
    {    
        public IAuthToken Token { get; set; }

        public Admin PearAdmin { get; set; }
    }
}
