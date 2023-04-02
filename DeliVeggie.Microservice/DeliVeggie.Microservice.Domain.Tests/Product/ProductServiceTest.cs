using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeliVeggie.Microservice.Data.Contracts.Product;
using DeliVeggie.Microservice.Domain.Contracts.Messaging;
using DeliVeggie.Microservice.Domain.Contracts.PriceReduction;
using DeliVeggie.Microservice.Domain.Contracts.Product;
using DeliVeggie.Microservice.Domain.Product;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeliVeggie.Microservice.Domain.Tests.Product
{
    [TestClass]
    public class ProductServiceTest
    {
        private Mock<IMessagingAdapter> _messagingAdapterMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IPriceReductionService> _priceReductionServiceMock;
        private IProductService _productServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _messagingAdapterMock = new Mock<IMessagingAdapter>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _priceReductionServiceMock = new Mock<IPriceReductionService>();
            _productServiceMock = new ProductService(_messagingAdapterMock.Object, null, null);
        }

        [TestMethod]
        public void RespondToProductMessages_ShouldCallMessagingAdapterRespondAsync()
        {
            //Arrange

            //Act
            _productServiceMock.RespondToProductMessages();

            //Assert
            _messagingAdapterMock.Verify(x => x.RespondAsync(It.IsAny<Func<IRequest, IResponse>>()), Times.Once);
        }

        [TestMethod]
        public void HandleRequest_ShouldReturnCorrectResponse_ForProductDetailsRequest()
        {
            //Arrange
            var product = GetProductDetails();
            _productRepositoryMock.Setup(x => x.GetProductById("1")).ReturnsAsync(product);
            _priceReductionServiceMock.Setup(x => x.GetPriceReductionByDayOfWeek(It.IsAny<int>())).ReturnsAsync(2);

            var productService = new ProductService(null, _productRepositoryMock.Object, _priceReductionServiceMock.Object);
            var request = new Request<ProductDetailsRequest>() { Data = new ProductDetailsRequest { Id = "1" } };

            //Act
            var response = CallPrivateMethod<IResponse>(productService, "HandleRequest", request);

            //Assert
            Assert.IsInstanceOfType(response, typeof(Response<ProductDetailsResponse>));
            var responseData = (response as Response<ProductDetailsResponse>).Data;
            Assert.AreEqual("1", responseData.Id);
            Assert.AreEqual(8, responseData.PriceWithReduction);
        }

        [TestMethod]
        public void HandleRequest_ShouldReturnCorrectResponse_ForGetAllProductsRequest()
        {
            //Arrange
            var products = GetProducts();
            var results = products.ToList();
            _productRepositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products);

            var productService = new ProductService(null, _productRepositoryMock.Object, null);
            var request = new Request<ProductsRequest>() { };

            //Act
            var response = CallPrivateMethod<IResponse>(productService, "HandleRequest", request);

            //Assert
            Assert.IsInstanceOfType(response, typeof(Response<List<ProductResponse>>));
            var responseData = (response as Response<List<ProductResponse>>).Data;
            Assert.AreEqual(results.Count, responseData.Count);
            Assert.AreEqual(results[0].Id, responseData[0].Id);
            Assert.AreEqual(results[1].Name, responseData[1].Name);
        }


        private ProductDetailsResponse GetProductDetails()
        {
            return new ProductDetailsResponse { Id = "1", Name = "Test Product", PriceWithReduction = 10 };
        }

        private IEnumerable<ProductResponse> GetProducts()
        {
            return new List<ProductResponse>
            {
                new ProductResponse { Id = "1", Name = "Test Product 1" },
                new ProductResponse { Id = "2", Name = "Test Product 2" }
            };
        }

        private static TReturn CallPrivateMethod<TReturn>(ProductService instance
                                                        ,string methodName,
                                                        params object[] parameters)
        {
            BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo method = typeof(ProductService).GetMethod(methodName, bindingAttr);

            return (TReturn)method.Invoke(instance, parameters);
        }
    }
}
