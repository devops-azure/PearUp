using System.Threading.Tasks;
using PearUp.BusinessEntity;
using PearUp.CommonEntities;
using PearUp.IBusiness;
using PearUp.Constants;
using System;
using PearUp.IRepository;
using PearUp.Authentication;
using PearUp.CommonEntity;
using System.Collections.Generic;
using PearUp.DTO;
using PearUp.Factories.Interfaces;
using MediatR;
using PearUp.Business.Events;
using PearUp.IDomainServices;

namespace PearUp.Business
{
    public class UserService : IUserService
    {
        public const string Atleast_One_Interest_Is_Required = "Atleast one interest is required";
        public const string Invalid_Interests =  "Invalid interests";
        public const string Failed_To_Update_Interests = "Failed to update interests";
        public const string Failed_To_Update_Location = "Failed to update location";
        public const string Failed_To_Save_User = "Failed to save user";
        public const string Location_Is_Required = "Location is required field";


        
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IIdentityProvider _userIdentityProvider;
        private readonly IMediator _mediator;
        private readonly IUserDomainService _userDomainService;

        public UserService(
            IUserRepository userRepository,
            IUserFactory userFactory,
            IIdentityProvider userIdentityProvider,
            IMediator mediator,
            IUserDomainService userDomainService
            )
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
            _userIdentityProvider = userIdentityProvider;
            _mediator = mediator;
            _userDomainService = userDomainService;
        }

        public async Task<Result<User>> GetUser(string phoneNumber)
        {
            var userResult = await _userRepository.GetUserByPhoneNumber(phoneNumber);
            if (userResult.IsSuccessed)
            {
                return Result.Ok(userResult.Value);
            }
            else
            {
                return Result.Fail<User>(userResult.GetErrorString());
            }
        }

        public async Task<Result> SetUserInterestsAsync(int[] interestIds)
        {
            if (interestIds == null || interestIds.Length <= 0)
            {
               return Result.Fail(Atleast_One_Interest_Is_Required);
            }

            var currentUserResult = await _userIdentityProvider.GetUser();
            if (!currentUserResult.IsSuccessed)
                return Result.Fail(currentUserResult.GetErrorString());

            var user = currentUserResult.Value;

            var setUserInterestsResult = await _userDomainService.SetUserInterests(user, interestIds);
            if (!setUserInterestsResult.IsSuccessed)
                return Result.Fail(setUserInterestsResult.GetErrorString());

            _userRepository.Update(user);

            var saveResult = await _userRepository.SaveChangesAsync(Failed_To_Update_Interests);

            if (!saveResult.IsSuccessed)
                return Result.Fail(saveResult.GetErrorString());

            _mediator.Publish(new UserModified(user));
            return Result.Ok();
        }

        public async Task<Result<User>> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO)
        {
            var existingUserResult = await _userRepository.GetUserByPhoneNumber(userRegistrationDTO.PhoneNumber);

            if (existingUserResult.IsSuccessed)
                return Result.Fail<User>(UserErrorMessages.User_Already_Exists_With_Given_Phone_Number);

            var userResult = await _userFactory.CreateUserAsync(userRegistrationDTO);
            if (!userResult.IsSuccessed)
                return Result.Fail<User>(userResult.GetErrorString());

            _userRepository.Create(userResult.Value);
            var saveResult = await _userRepository.SaveChangesAsync(Failed_To_Save_User);

            if (!saveResult.IsSuccessed)
                return Result.Fail<User>(saveResult.GetErrorString());

            _mediator.Publish(new UserCreated(userResult.Value));

            return Result.Ok(userResult.Value);
        }

        public async Task<Result> SetLocationAsync(UserLocationDTO userLocationDTO)
        {

            if(userLocationDTO == null)
            {
                return Result.Fail(Location_Is_Required);
            }
            var currentUserResult = await _userIdentityProvider.GetUser();
            if (!currentUserResult.IsSuccessed)
                return Result.Fail(currentUserResult.GetErrorString());
          
            var user = currentUserResult.Value;
            
            var locationResult = Location.Create(userLocationDTO.Latitude, userLocationDTO.Longitude);
            if (!locationResult.IsSuccessed)
                return Result.Fail(locationResult.GetErrorString());

            user.SetLocation(locationResult.Value);
            _userRepository.Update(user);

            var saveResult = await _userRepository.SaveChangesAsync(Failed_To_Update_Location);

            if (!saveResult.IsSuccessed)
                return Result.Fail(saveResult.GetErrorString());

            _mediator.Publish(new UserModified(user));
            return Result.Ok();
        }

    }
}
