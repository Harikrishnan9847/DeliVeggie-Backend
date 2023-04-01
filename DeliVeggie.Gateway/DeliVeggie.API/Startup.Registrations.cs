using DeliVeggie.API.Filter;
using DeliVeggie.API.Middleware;
using DeliVeggie.Domain.Contracts.Messaging;
using DeliVeggie.Domain.Contracts.Product;
using DeliVeggie.Domain.Messaging;
using DeliVeggie.Domain.Product;
using DeliVeggie.EasyNetQ.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace DeliVeggie.API
{
    public partial class Startup
    {
        internal static class Registrations
        {
            public static void Register(IServiceCollection services)
            {
                services.AddTransient<ExceptionHandler>();
                services.AddTransient<ProductIdFilter>();
                services.AddTransient<CustomProductIdFilter>();
                services.AddTransient<IProductService,ProductService>();
                services.AddSingleton<IMessagingAdapter,EasyNetQMessagingAdapter>();
                services.AddSingleton<IEasyNetQBus,EasyNetQBus>();
            }
        }
    }
}
