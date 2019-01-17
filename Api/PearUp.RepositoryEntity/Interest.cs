using System.Collections.Generic;

namespace PearUp.RepositoryEntity
{
    public class Interest : Audit
    {
        public Interest()
        {
            UserInterests = new HashSet<UserInterest>();
        }
        public string InterestName { get; set; }
        public string InterestDescription { get; set; }


        public ICollection<UserInterest> UserInterests { get; set; }
    }
}