using System.Collections.Generic;
using System.Threading.Tasks;
using DeliVeggie.Microservice.Models.Model;

namespace DeliVeggie.Microservice.Data.Contracts.PriceReduction
{
    public interface IPriceReductionRepository
    {
        Task<List<PriceReductions>> GetPriceReductions();
        Task<bool> CheckIfDocumentExists();
        Task SavePriceReductionsAsync();
    }
}
