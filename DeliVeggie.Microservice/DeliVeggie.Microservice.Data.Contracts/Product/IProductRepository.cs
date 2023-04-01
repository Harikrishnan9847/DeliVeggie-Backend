using System.Collections.Generic;
using System.Threading.Tasks;
using DeliVeggie.Models.Response;

namespace DeliVeggie.Microservice.Data.Contracts.Product
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductResponse>> GetProductsAsync();
        Task<bool> CheckIfDocumentExists();
        Task<ProductDetailsResponse> GetProductById(string id);
        Task SaveProductsAsync();
    }
}
