using AutoMapper;
using RandApp.DTOs;
using RandApp.Models;

namespace RandApp.Mapper
{
    public class ObjMapper : Profile
    {
        public ObjMapper()
        {
            CreateMap<Item, ItemDto>()
                .ForMember(o => o.Color, k => k.MapFrom(m => m.Color))
                .ForMember(o => o.Size, k => k.MapFrom(m => m.Size))
                .ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(o => o.ShoppingCartList, k => k.MapFrom(m => m.ShoppingCartList))
                .ReverseMap();
            CreateMap<CartItem, CartItemDto>()
                .ReverseMap();
            CreateMap<ItemColors, ItemColorsDto>()
                .ReverseMap();
            CreateMap<ItemSizes, ItemSizesDto>()
                .ReverseMap();
        }
    }
}
