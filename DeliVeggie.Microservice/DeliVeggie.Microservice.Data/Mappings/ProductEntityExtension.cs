using System.Collections.Generic;
using AutoMapper;
using DeliVeggie.Microservice.Data.Entity;
using DeliVeggie.Models.Response;

namespace ProductsApp.Data.Extensions
{
    public static class ProductEntityExtension
    {
        public static IEnumerable<ProductResponse> ToProductResponse(this IEnumerable<ProductEntity> products, 
                                                                     IMapper mapper)
        {
            return mapper.Map<IEnumerable<ProductResponse>>(products);
        }

        public static ProductDetailsResponse ToProductDetailsResponse(this ProductEntity product,
                                                                                   IMapper mapper)
        {
            return mapper.Map<ProductDetailsResponse>(product);
        }
    }
}
