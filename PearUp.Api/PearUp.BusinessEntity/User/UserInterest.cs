using PearUp.CommonEntities;
using System.Collections.Generic;

namespace PearUp.BusinessEntity
{
    public class UserInterest
    {
        private UserInterest()
        {

        }

        private UserInterest(int interestId)
        {
            this.InterestId = interestId;
        }

        public int UserId { get; set; }
        public int InterestId { get; set; }

        public override bool Equals(object obj)
        {
            return this.InterestId == ((UserInterest)obj).InterestId;
        }

        public override int GetHashCode()
        {
            return this.InterestId.GetHashCode();
        }

        public static Result<UserInterest> Create(int interestId)
        {
            return Result.Ok(new UserInterest(interestId));
        }
    }
}
