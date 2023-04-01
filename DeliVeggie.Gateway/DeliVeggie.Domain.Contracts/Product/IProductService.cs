using System.Threading.Tasks;
using DeliVeggie.Models.Response;

namespace DeliVeggie.Domain.Contracts.Product
{
    public interface IProductService
    {
        Task<IResponse> GetAllProducts();
        Task<IResponse> GetProduct(string id);
    }
}
