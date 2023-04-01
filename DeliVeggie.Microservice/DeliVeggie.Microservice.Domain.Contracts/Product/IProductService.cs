using System.Threading.Tasks;

namespace DeliVeggie.Microservice.Domain.Contracts.Product
{
    public interface IProductService
    {
        void RespondToProductMessages();
        Task InserProductsIfNotExist();
    }
}
