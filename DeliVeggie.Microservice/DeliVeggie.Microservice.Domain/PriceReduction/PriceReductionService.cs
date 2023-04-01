using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliVeggie.Microservice.Data.Contracts.PriceReduction;
using DeliVeggie.Microservice.Domain.Contracts.PriceReduction;
using DeliVeggie.Microservice.Models.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace DeliVeggie.Microservice.Domain.PriceReduction
{
    public class PriceReductionService : IPriceReductionService
    {
        #region PRIVATE PROPERTIES
        private readonly IPriceReductionRepository _priceReductionRepository;
        private readonly int _hours = 0;
        private IMemoryCache _cache;
        #endregion

        #region CONSTRUCTOR
        public PriceReductionService(IPriceReductionRepository priceReductionRepository, 
                                     IMemoryCache cache,
                                     IConfiguration configuration)
        {
            _priceReductionRepository = priceReductionRepository;
            _cache = cache;
            _hours = int.Parse(configuration.GetSection("CacheSettings")
                .GetChildren()
                .FirstOrDefault(config => config.Key == "Hours")
                .Value);
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<IEnumerable<PriceReductions>> GetPriceReductions()
        {
            List<PriceReductions> priceReductions = await GetPriceReductionsFromCache();
            return priceReductions;
        }

        public async Task<double> GetPriceReductionByDayOfWeek(int dayOfWeek)
        {
            List<PriceReductions> priceReductions = await GetPriceReductionsFromCache();

            return priceReductions.Where(reduction => reduction.DayOfWeek == dayOfWeek)
                                  .FirstOrDefault()
                                  .Reduction;
        }

        public async Task InserPriceReductionsIfNotExist()
        {
            var isDocumentExist = await _priceReductionRepository.CheckIfDocumentExists();
            if (isDocumentExist)
            {
                return;
            }
            await _priceReductionRepository.SavePriceReductionsAsync();
        }
        #endregion

        #region PRIVATE METHODS
        private async Task<List<PriceReductions>> GetPriceReductionsFromCache()
        {
            var isExist = _cache.TryGetValue("PriceReductions", out List<PriceReductions> priceReductions);
            if (!isExist)
            {
                priceReductions = await _priceReductionRepository.GetPriceReductions();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(_hours));
                _cache.Set("PriceReductions", priceReductions, cacheEntryOptions);
            }

            return priceReductions;
        }
        #endregion
    }
}
