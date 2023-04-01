using System;
using System.Threading.Tasks;
using DeliVeggie.Domain.Contracts.Messaging;
using DeliVeggie.EasyNetQ.RabbitMQ;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;

namespace DeliVeggie.Domain.Messaging
{
    public class EasyNetQMessagingAdapter : IMessagingAdapter
    {
        #region PRIVATE PROPERTIES
        private readonly IEasyNetQBus _easyNetQBus;
        #endregion

        #region CONSTRUCTOR
        public EasyNetQMessagingAdapter(IEasyNetQBus easyNetQBus)
        {
            _easyNetQBus = easyNetQBus;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<IResponse> RequestAsync(IRequest request)
        {
            return await _easyNetQBus.RequestAsync(request);
        }

        public void RespondAsync(Func<IRequest, IResponse> data)
        {
            _easyNetQBus.RespondAsync(data);
        }
        #endregion
    }
}
