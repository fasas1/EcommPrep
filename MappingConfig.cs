using AutoMapper;
using DesignArch.Entities;
using DesignArch.Entities.Dto;

namespace DesignArch
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserOrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
        }
    }
}
