using System.Collections.Generic;
using System.Threading.Tasks;
using DeliVeggie.Microservice.Models.Model;

namespace DeliVeggie.Microservice.Domain.Contracts.PriceReduction
{
    public interface IPriceReductionService
    {
        Task<IEnumerable<PriceReductions>> GetPriceReductions();
        Task<double> GetPriceReductionByDayOfWeek(int dayOfWeek);
        Task InserPriceReductionsIfNotExist();
    }
}
