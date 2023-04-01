using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace DeliVeggie.API.Filter
{
    public class ProductIdFilter : ServiceFilterAttribute
    {
        public ProductIdFilter() : base(typeof(CustomProductIdFilter)) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CustomProductIdFilter : ActionFilterAttribute
    {
        private readonly ILogger<CustomProductIdFilter> _logger;

        public CustomProductIdFilter(ILogger<CustomProductIdFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var routeData = filterContext.HttpContext.GetRouteData();

            var id = routeData?.Values["id"]?.ToString();

            if (string.IsNullOrWhiteSpace(id))
            {               
                _logger.LogError("Empty Input", routeData);
                throw new Exception("The parameter is not valid!");
            }
        }

    }
}
