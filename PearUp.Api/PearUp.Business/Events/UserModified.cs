using MediatR;
using PearUp.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PearUp.Business.Events
{
    public class UserModified : INotification
    {
        public readonly User User;

        public UserModified(User user)
        {
            User = user;
        }
    }
}
