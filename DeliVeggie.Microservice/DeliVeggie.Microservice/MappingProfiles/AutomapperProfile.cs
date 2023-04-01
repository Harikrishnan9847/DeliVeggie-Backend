using AutoMapper;
using DeliVeggie.Microservice.Data.Entity;
using DeliVeggie.Models.Response;

namespace DeliVeggie.Microservice.MappingProfiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ProductEntity, ProductResponse>();
            CreateMap<ProductEntity, ProductDetailsResponse>()
                .ForMember(dest => dest.PriceWithReduction, 
                            opt => opt.MapFrom(src => src.Price));
        }
    }
}
