using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.DTOs.User;
using BookManagement.ViewModels.Auth;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Cart;
using BookManagement.ViewModels.User;

namespace BookManagement.Mappings
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile()
        {
            CreateMap<LoginViewModel, LoginDto>();
            CreateMap<RegisterViewModel, RegisterDto>();
            CreateMap<UserViewModel, UserDto>().ReverseMap();
            CreateMap<CreateUserViewModel, CreateUserDto>().ReverseMap();
            CreateMap<EditUserViewModel, UserUpdateDto>().ReverseMap();
            CreateMap<UserDto, EditUserViewModel>().ReverseMap();

            // Cart
            CreateMap<CartItem, CartItemViewModel>()
                    .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                    .ForMember(dest => dest.BookImageUrl, opt => opt.MapFrom(src => src.Book.ImageUrl))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Book.Price));

            // Book 
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}
