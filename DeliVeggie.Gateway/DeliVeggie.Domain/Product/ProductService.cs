using System.Threading.Tasks;
using DeliVeggie.Domain.Contracts.Messaging;
using DeliVeggie.Domain.Contracts.Product;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;

namespace DeliVeggie.Domain.Product
{
    public class ProductService : IProductService
    {
        #region PRIVATE PROPERTIES
        private readonly IMessagingAdapter _messagingAdapter;
        #endregion

        #region CONSTRUCTOR
        public ProductService(IMessagingAdapter messagingAdapter)
        {
            _messagingAdapter = messagingAdapter;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<IResponse> GetAllProducts()
        {
            var request = new Request<ProductsRequest>() 
            { 
                Data = new ProductsRequest() 
            };
            return await _messagingAdapter.RequestAsync(request);
        }

        public async Task<IResponse> GetProduct(string id)
        {
            var request = new Request<ProductDetailsRequest>() 
            { 
                Data = new ProductDetailsRequest() 
                { 
                    Id = id 
                } 
            };
            return await _messagingAdapter.RequestAsync(request);
        }
        #endregion

    }
}
