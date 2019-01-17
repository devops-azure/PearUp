using System.Threading.Tasks;
using MediatR;
using PearUp.Business.Events;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.Constants;
using PearUp.IBusiness;
using PearUp.IDomainServices;
using PearUp.IRepository;

namespace PearUp.Business
{
    public class UserPhotoService : IUserPhotoService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public const string Failed_To_Update_Photos = "Failed to update photos";
        public UserPhotoService(IUserRepository userRepository, IMediator mediator)
        {
            this._userRepository = userRepository;
            this._mediator = mediator;
        }
        /// <summary>
        /// Add Photos to User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="path"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Result> SetUserPhotoAsync(int userId, string path, int order)
        {
            var result = UserPhoto.Create(order, path);
            if (!result.IsSuccessed)
                return Result.Fail(result.GetErrorString());

            var userResult = await _userRepository.GetUserByIdAsync(userId);
            if (!userResult.IsSuccessed)
                return Result.Fail(userResult.GetErrorString());

            var user = userResult.Value;
            var userPhoto = result.Value;
            user.SetPhoto(userPhoto);
            _userRepository.Update(user);

            var saveResult = await _userRepository.SaveChangesAsync(Failed_To_Update_Photos);
            if (!saveResult.IsSuccessed)
                return Result.Fail(saveResult.GetErrorString());

            _mediator.Publish(new UserModified(user));
            return Result.Ok();
        }
    }
}
