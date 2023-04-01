using System;
using System.IO;
using System.Threading.Tasks;
using DeliVeggie.EasyNetQ.RabbitMQ;
using DeliVeggie.Microservice.Data.Contracts.PriceReduction;
using DeliVeggie.Microservice.Data.Contracts.Product;
using DeliVeggie.Microservice.Data.PriceReduction;
using DeliVeggie.Microservice.Data.Product;
using DeliVeggie.Microservice.Domain.Contracts.Messaging;
using DeliVeggie.Microservice.Domain.Contracts.PriceReduction;
using DeliVeggie.Microservice.Domain.Contracts.Product;
using DeliVeggie.Microservice.Domain.Messaging;
using DeliVeggie.Microservice.Domain.PriceReduction;
using DeliVeggie.Microservice.Domain.Product;
using DeliVeggie.Microservice.MappingProfiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliVeggie.Microservice
{
    internal class Program
    {
        #region PRIVATE PROPERTIES
        private static ServiceProvider _services;
        private static IProductService _productService;
        private static IPriceReductionService _priceReductionService;
        #endregion

        #region MAIN METHOD
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            Console.WriteLine("Microservice Starting...");

            IConfigurationRoot configuration = BuildConfiguration();
            InjectDependencies(configuration);

            _productService = _services.GetService<IProductService>();
            _priceReductionService = _services.GetService<IPriceReductionService>();

            await InsertMockData();

            ListenToProductMessages();
        }
        #endregion

        #region PRIVATE METHODS
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            return configuration;
        }

        private static void InjectDependencies(IConfigurationRoot configuration)
        {
            _services = new ServiceCollection()
                            .AddSingleton<IConfiguration>(configuration)
                            .AddMemoryCache()
                            .AddSingleton<IMessagingAdapter, EasyNetQMessagingAdapter>()
                            .AddSingleton<IEasyNetQBus, EasyNetQBus>()
                            .AddSingleton<IProductService, ProductService>()
                            .AddSingleton<IPriceReductionService, PriceReductionService>()
                            .AddSingleton<IProductRepository, ProductRepository>()
                            .AddSingleton<IPriceReductionRepository, PriceReductionRepository>()
                            .AddAutoMapper(typeof(AutomapperProfile))
                            .BuildServiceProvider();
        }

        private static async Task InsertMockData()
        {
            await _productService.InserProductsIfNotExist();
            await _priceReductionService.InserPriceReductionsIfNotExist();
        }

        private static void ListenToProductMessages()
        {
            int counter = 0;
            while (true)
            {
                if (counter == 0)
                {
                    _productService.RespondToProductMessages();
                    counter = 1;
                }
            }
        }
        #endregion
    }
}
