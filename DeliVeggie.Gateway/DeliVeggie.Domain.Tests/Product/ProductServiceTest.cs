using System.Threading.Tasks;
using DeliVeggie.Domain.Contracts.Messaging;
using DeliVeggie.Domain.Product;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DeliVeggie.Domain.Tests.Product
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IMessagingAdapter> _messagingAdapterMock;
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            _messagingAdapterMock = new Mock<IMessagingAdapter>();
            _productService = new ProductService(_messagingAdapterMock.Object);
        }

        [TestMethod]
        public async Task GetAllProducts_Should_CallRequestAsync_WithCorrectRequest()
        {
            // Arrange
            var expectedRequest = new Request<ProductsRequest> { Data = new ProductsRequest() };
            var expectedResponse = new Response<ProductResponse> { Data = new ProductResponse() };

            _messagingAdapterMock.Setup(m => m.RequestAsync(expectedRequest)).ReturnsAsync(expectedResponse);

            // Act
            var response = await _productService.GetAllProducts();

            // Assert
            _messagingAdapterMock.Verify(m => m.RequestAsync(It.IsAny<Request<ProductsRequest>>()),Times.Once);
        }

        [TestMethod]
        public async Task GetProduct_Should_CallRequestAsync_WithCorrectRequest()
        {
            // Arrange
            var expectedRequest = new Request<ProductDetailsRequest>() { Data = new ProductDetailsRequest() { Id = "testId" } };
            _messagingAdapterMock.Setup(m => m.RequestAsync(expectedRequest)).ReturnsAsync(new Response<ProductDetailsResponse>());

            // Act
            var response = await _productService.GetProduct("testId");

            // Assert
            _messagingAdapterMock.Verify(m => m.RequestAsync(It.IsAny<Request<ProductDetailsRequest>>()), Times.Once);
        }
    }

}
