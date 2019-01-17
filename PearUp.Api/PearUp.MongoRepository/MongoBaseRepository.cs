
using AutoMapper;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace PearUp.MongoRepository
{
    public class MongoBaseRepository<TDomain> 
    {

        private readonly IMapper _mapper;
        private readonly string _collectionName;
        protected IMongoDatabase _dataBase;
        internal MongoBaseRepository(PearUpMongoContext pearUpMangoContext, string collectionName)
        {
            this._dataBase = pearUpMangoContext.GetDataBase();
            this._collectionName = collectionName;
        }

        //protected TDomain Map(TBusiness entity)
        //{
        //    return _mapper.Map<TBusiness, TDomain>(entity);
        //}

        public virtual async Task CreateAsync(TDomain entity)
        {
            var userCollection = GetCollection();
            await userCollection.InsertOneAsync(entity);
        }

        //private async Task Create(TDomain entity)
        //{
        //    var userCollection = GetCollection();
        //    await userCollection.InsertOneAsync(entity);
        //}

        protected IMongoCollection<TDomain> GetCollection()
        {
            return _dataBase.GetCollection<TDomain>(_collectionName);
        }
    }
}
