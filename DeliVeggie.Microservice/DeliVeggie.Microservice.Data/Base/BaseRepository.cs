using System.Linq;
using DeliVeggie.Microservice.Data.Contracts.Repository.Base;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DeliVeggie.Microservice.Data.Base
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private const string CONNECTION = "mongoConnectionString";
        private const string DATABASE_SECTION_NAME = "MongoConfiguration";
        protected MongoClient Client;
        protected IMongoDatabase Database;


        public BaseRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(CONNECTION);
            var mongoSettings = configuration.GetSection(DATABASE_SECTION_NAME)
                                             .GetChildren()
                                             .ToDictionary(x => x.Key, x => x.Value);
            var databaseName = mongoSettings["Database"];
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
