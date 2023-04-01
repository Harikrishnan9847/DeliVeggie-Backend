using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliVeggie.Microservice.Data.Base;
using DeliVeggie.Microservice.Data.Contracts.PriceReduction;
using DeliVeggie.Microservice.Models.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DeliVeggie.Microservice.Data.PriceReduction
{
    public class PriceReductionRepository : BaseRepository<PriceReductions>, IPriceReductionRepository
    {
        #region PRIVATE PROPERTIES
        private const string COLLECTION = "PriceReductions";
        #endregion

        #region CONSTRUCTOR
        public PriceReductionRepository(IConfiguration configuration) : base(configuration) { }
        #endregion

        #region PUBLIC METHODS
        public async Task<List<PriceReductions>> GetPriceReductions()
        {
            var productCollection = Database.GetCollection<PriceReductions>(COLLECTION);
            var products = await Database.GetCollection<PriceReductions>(COLLECTION).Find(_ => true)
                                                                                    .ToListAsync();

            return products.ToList();
        }

        public async Task<bool> CheckIfDocumentExists()
        {
            ProjectionDefinition<PriceReductions> projection = Builders<PriceReductions>.Projection.Include(x => x.DayOfWeek);
            var priceReductions = await Database.GetCollection<PriceReductions>(COLLECTION).Find(_ => true)
                                                                                  .Project<PriceReductions>(projection)
                                                                                  .Limit(1)
                                                                                  .ToListAsync();

            if (priceReductions.Any() && priceReductions != null)
            {
                return true;
            }
            return false;
        }

        public async Task SavePriceReductionsAsync()
        {
            await Database.GetCollection<PriceReductions>(COLLECTION)
                          .InsertManyAsync(GetDummyPriceReductions());
        }
        #endregion

        #region PRIVATE METHODS
        private IEnumerable<PriceReductions> GetDummyPriceReductions()
        {
            return new List<PriceReductions>()
            {
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 1,
                    Reduction = 0
                },
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 2,
                    Reduction = 0.2
                },
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 3,
                    Reduction = 0.4
                },
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 4,
                    Reduction = 0.1
                },
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 5,
                    Reduction = 0
                },
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 6,
                    Reduction = 0.5
                },
                new PriceReductions()
                {
                    _id = Guid.NewGuid(),
                    DayOfWeek = 7,
                    Reduction = 0.3
                }
            };
        }
        #endregion
    }
}
