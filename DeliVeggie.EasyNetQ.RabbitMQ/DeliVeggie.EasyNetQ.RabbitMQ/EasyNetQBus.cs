using System;
using System.Threading.Tasks;
using DeliVeggie.Models.Request;
using DeliVeggie.Models.Response;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace DeliVeggie.EasyNetQ.RabbitMQ
{
    public class EasyNetQBus : IEasyNetQBus
    {
        #region PRIVATE VARIABLES
        private readonly IBus _bus;
        #endregion

        #region CONSTANT
        private const string RABBIT_MQ_CONNECTION_STRING = "rabbitMqConnectionString";
        #endregion

        #region CONSTRUCTOR
        public EasyNetQBus(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(RABBIT_MQ_CONNECTION_STRING);
            _bus = RabbitHutch.CreateBus(connectionString);
        }
        #endregion
        public async Task<IResponse> RequestAsync(IRequest request)
        {
            return await _bus.Rpc.RequestAsync<IRequest, IResponse>(request);
        }

        public async void RespondAsync(Func<IRequest, IResponse> data)
        {
            await _bus.Rpc.RespondAsync(data);
        }
    }
}
