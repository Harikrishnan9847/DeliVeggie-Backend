using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DeliVeggie.Microservice.Data.Base;
using DeliVeggie.Microservice.Data.Contracts.Product;
using DeliVeggie.Microservice.Data.Entity;
using DeliVeggie.Models.Response;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductsApp.Data.Extensions;

namespace DeliVeggie.Microservice.Data.Product
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        #region PRIVATE PROPERTIES
        private const string COLLECTION = "Products";
        private IMapper _mapper;
        #endregion

        #region CONSTRUCTOR
        public ProductRepository(IConfiguration configuration, IMapper mapper) : base(configuration)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
        {
            ProjectionDefinition<ProductEntity> projection = Builders<ProductEntity>.Projection.Include(x => x.Id)
                                                                                               .Include(x => x.Name);
            var products = await Database.GetCollection<ProductEntity>(COLLECTION).Find(_ => true)
                                                                                  .Project<ProductEntity>(projection)
                                                                                  .ToListAsync();

            return products.ToProductResponse(_mapper);
        }

        public async Task<bool> CheckIfDocumentExists()
        {
            ProjectionDefinition<ProductEntity> projection = Builders<ProductEntity>.Projection.Include(x => x.Id);
            var products = await Database.GetCollection<ProductEntity>(COLLECTION).Find(_ => true)
                                                                                  .Project<ProductEntity>(projection)
                                                                                  .Limit(1)
                                                                                  .ToListAsync();

            if (products.Any() && products != null)
            {
                return true;
            }
            return false;
        }

        public async Task<ProductDetailsResponse> GetProductById(string id)
        {
            var filter = Builders<ProductEntity>.Filter.Eq(p => p.Id, ObjectId.Parse(id));
            var products = await Database.GetCollection<ProductEntity>(COLLECTION).Find(filter)
                                                                                  .FirstOrDefaultAsync();

            return products?.ToProductDetailsResponse(_mapper);
        }

        public async Task SaveProductsAsync()
        {
            await Database.GetCollection<ProductEntity>(COLLECTION)
                          .InsertManyAsync(CreateDummyProducts());
        }
        #endregion

        #region PRIVATE METHODS
        private IEnumerable<ProductEntity> CreateDummyProducts()
        {
            return new List<ProductEntity>
            {
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "A pile of potatoes",
                    EntryDate = DateTime.Parse("01-01-2021"),
                    Price = 10.21
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Organic Carrots",
                    EntryDate = DateTime.Parse("03-01-2021"),
                    Price = 2.99
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Fresh Broccoli",
                    EntryDate = DateTime.Parse("07-01-2021"),
                    Price = 1.99
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Kale Salad Mix",
                    EntryDate = DateTime.Parse("11-01-2021"),
                    Price = 4.50
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Spinach Bunch",
                    EntryDate = DateTime.Parse("26-12-2020"),
                    Price = 1.25
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Red Bell Pepper",
                    EntryDate = DateTime.Parse("15-06-2020"),
                    Price = 0.99
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Organic Cucumber",
                    EntryDate = DateTime.Parse("03-03-2021"),
                    Price = 2.50
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Zucchini Squash",
                    EntryDate = DateTime.Parse("17-06-2021"),
                    Price = 0.75
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Yellow Onion",
                    EntryDate = DateTime.Parse("26-04-2022"),
                    Price = 0.69
                },
                new ProductEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Tomato Vine",
                    EntryDate = DateTime.Parse("06-02-2021"),
                    Price = 1.49
                }
            };
        }
        #endregion
    }
}
