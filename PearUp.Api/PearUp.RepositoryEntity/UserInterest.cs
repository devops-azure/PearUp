using MongoDB.Bson.Serialization.Attributes;

namespace PearUp.RepositoryEntity
{
    public class UserInterest : Audit
    {
        public int UserId { get; set; }
        public int InterestId { get; set; }

        [BsonIgnore]
        public Interest Interest { get; set; }
        [BsonIgnore]
        public User User { get; set; }
    }
}
