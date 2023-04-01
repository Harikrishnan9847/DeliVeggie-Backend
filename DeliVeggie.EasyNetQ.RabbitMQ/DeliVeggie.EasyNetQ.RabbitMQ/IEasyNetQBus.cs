using System;
using System.Threading.Tasks;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;

namespace DeliVeggie.EasyNetQ.RabbitMQ
{
    public interface IEasyNetQBus
    {
        Task<IResponse> RequestAsync(IRequest request);
        void RespondAsync(Func<IRequest, IResponse> data);
    }
}
