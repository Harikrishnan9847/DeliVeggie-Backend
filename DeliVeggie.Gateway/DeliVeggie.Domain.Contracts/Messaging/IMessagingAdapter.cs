using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;
using System.Threading.Tasks;
using System;

namespace DeliVeggie.Domain.Contracts.Messaging
{
    public interface IMessagingAdapter
    {
        Task<IResponse> RequestAsync(IRequest request);
        void RespondAsync(Func<IRequest, IResponse> data);
    }
}
