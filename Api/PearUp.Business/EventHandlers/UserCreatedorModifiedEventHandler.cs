using MediatR;
using PearUp.Business.Events;
using PearUp.BusinessEntity;
using PearUp.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PearUp.Business.EventHandlers
{
    public class UserCreatedorModifiedEventHandler : INotificationHandler<UserModified>, INotificationHandler<UserCreated>
    {
        private readonly IMongoUserRepository _userRepository;

        public UserCreatedorModifiedEventHandler(IMongoUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task Handle(UserModified notification, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.ReplaceUserAsync(notification.User);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
        {
            await _userRepository.CreateAsync(notification.User);
        }
    }
}
