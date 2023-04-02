using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliVeggie.Microservice.Data.Contracts.PriceReduction;
using DeliVeggie.Microservice.Domain.PriceReduction;
using DeliVeggie.Microservice.Models.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeliVeggie.Microservice.Domain.Tests.PriceReduction
{
    [TestClass]
    public class PriceReductionServiceTests
    {
        private Mock<IPriceReductionRepository> _priceReductionRepositoryMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private Mock<IConfiguration> _configurationMock;
        private PriceReductionService _priceReductionService;
        [TestInitialize]
        public void Setup()
        {
            _priceReductionRepositoryMock = new Mock<IPriceReductionRepository>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x.GetSection("CacheSettings"))
                              .Returns(new ConfigurationSectionStub());
            _priceReductionService = new PriceReductionService(_priceReductionRepositoryMock.Object,
                                                               _memoryCacheMock.Object,
                                                               _configurationMock.Object);
        }

        [TestMethod]
        public async Task GetPriceReductions_ReturnsPriceReductionsFromCache()
        {
            // Arrange
            object priceReductions = GetPriceReductions();
            _memoryCacheMock.Setup(m => m.TryGetValue("PriceReductions", out priceReductions)).Returns(true);

            // Act
            var result = await _priceReductionService.GetPriceReductions();

            // Assert
            CollectionAssert.AreEqual(priceReductions as List<PriceReductions>, result.ToList());
        }

        [TestMethod]
        public async Task GetPriceReductions_ReturnsPriceReductionsFromRepositoryAndCachesIt()
        {
            // Arrange
            object priceReductions = GetPriceReductions();
            _memoryCacheMock.Setup(m => m.TryGetValue("PriceReductions", out priceReductions)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
            _priceReductionRepositoryMock.Setup(m => m.GetPriceReductions()).ReturnsAsync(priceReductions as List<PriceReductions>);

            // Act
            var result = await _priceReductionService.GetPriceReductions();

            // Assert
            CollectionAssert.AreEqual(priceReductions as List<PriceReductions>, result.ToList());
        }

        [TestMethod]
        public async Task GetPriceReductionByDayOfWeek_ReturnsPriceReductionForDayOfWeek()
        {
            // Arrange
            object priceReductions = GetPriceReductions();
            _memoryCacheMock.Setup(m => m.TryGetValue("PriceReductions", out priceReductions)).Returns(true);

            // Act
            var result = await _priceReductionService.GetPriceReductionByDayOfWeek(1);

            // Assert
            Assert.AreEqual(0.1, result);
        }

        private List<PriceReductions> GetPriceReductions()
        {
            return new List<PriceReductions>
            {
                new PriceReductions { DayOfWeek = 0, Reduction = 0.2 },
                new PriceReductions { DayOfWeek = 1, Reduction = 0.1 }
            };
        }
    }

    public class ConfigurationSectionStub : IConfigurationSection
    {
        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Key { get; set; }

        public string Path => throw new NotImplementedException();

        public string Value => throw new NotImplementedException();

        string IConfigurationSection.Value { get => "12"; set => throw new NotImplementedException(); }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>
            {
                new ConfigurationSectionStub()
                {
                    Key = "Hours"
                }
            };
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }


}
