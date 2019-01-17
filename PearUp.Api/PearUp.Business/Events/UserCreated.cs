using MediatR;
using PearUp.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.Business.Events
{
    public class UserCreated : INotification
    {
        public readonly User User;

        public UserCreated(User user)
        {
            User = user;
        }
    }
}
