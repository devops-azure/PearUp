using AutoMapper;
using MongoDB.Driver;
using PearUp.BusinessEntity;
using PearUp.Constants;
using PearUp.IRepository;
using System.Threading.Tasks;


namespace PearUp.MongoRepository
{
    public class MongoUserRepository : MongoBaseRepository<BusinessEntity.User>, IMongoUserRepository
    {
        private readonly IMapper _mapper;

        public MongoUserRepository(PearUpMongoContext pearUpMangoContext) : base(pearUpMangoContext,
             MongoCollectionConstants.User)
        {
        }

        public async Task ReplaceUserAsync(User user)
        {
            var userCollection = GetCollection();
            if (user != null)
            {
                await userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
        }
    }
}