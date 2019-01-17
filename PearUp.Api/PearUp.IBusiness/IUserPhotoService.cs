using PearUp.CommonEntities;
using System.Threading.Tasks;

namespace PearUp.IBusiness
{
    public interface IUserPhotoService
    {
       Task<Result> SetUserPhotoAsync(int userId, string path, int order);
    }
}
