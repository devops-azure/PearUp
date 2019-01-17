using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PearUp.CommonEntity;
using System.Security.Authentication;

namespace PearUp.MongoRepository
{
    public class PearUpMongoContext
    {
        private readonly string _connectionString;
        private static MongoClient _mongoClient = null;
        private static readonly object padlock = new object();
        private readonly string _dataBaseName;

        public PearUpMongoContext(IOptions<MongoSettings> settings)
        {
            this._connectionString = settings.Value.ConnectionString;
            this._dataBaseName = settings.Value.DataBaseName;
        }
        private MongoClient mongoClient
        {
            get
            {
                if (_mongoClient == null)
                {
                    lock (padlock)
                    {
                        if (_mongoClient == null)
                        {
                            _mongoClient = GetMongoClient();
                        }
                    }
                }
                return _mongoClient;
            }
        }
        private MongoClient GetMongoClient()
        {

            MongoClientSettings clientSettings = MongoClientSettings.FromUrl(new MongoUrl(_connectionString));
            clientSettings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            return new MongoClient(clientSettings);
        }

        public IMongoDatabase GetDataBase()
        {
            return mongoClient.GetDatabase(_dataBaseName);
        }
    }
}
