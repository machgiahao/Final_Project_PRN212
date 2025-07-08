using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.BusinessObjects.Enum;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.DTOs.Book;
using BookManagement.Services.DTOs.User;
using BookManagement.ViewModels.Auth;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Cart;
using BookManagement.ViewModels.Order;
using BookManagement.ViewModels.User;

namespace BookManagement.Mappings
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile()
        {
            // User
            CreateMap<LoginViewModel, LoginDto>();
            CreateMap<RegisterViewModel, RegisterDto>();
            CreateMap<UserViewModel, UserDto>().ReverseMap();
            CreateMap<CreateUserViewModel, CreateUserDto>().ReverseMap();
            CreateMap<EditUserViewModel, UserUpdateDto>().ReverseMap();
            CreateMap<UserViewModel, UserUpdateDto>().ReverseMap();
            CreateMap<UserDto, EditUserViewModel>().ReverseMap();

            // Cart
            CreateMap<CartItem, CartItemViewModel>()
                    .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                    .ForMember(dest => dest.BookImageUrl, opt => opt.MapFrom(src => src.Book.ImageUrl))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Book.Price))
                    .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Book.Stock));

            // Book 
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.StatusDisplay, opt => opt.MapFrom(src =>
                    src.Status == BookStatus.Avaiable ? "Available" :
                    src.Status == BookStatus.SoldOut ? "Sold Out" :
                    src.Status == BookStatus.Hide ? "Hidden" : "Unknown"
                ));
            CreateMap<BookFilterViewModel, BookPagedQueryDto>().ReverseMap();

            // Order
            CreateMap<PurchaseViewModel, Order>()
                .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.RecipientName))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()) 
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

            CreateMap<CartItemViewModel, OrderDetail>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetailId, opt => opt.Ignore());

            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src =>
                    src.Status == 0 ? "Pending" :
                    src.Status == 1 ? "Confirmed" :
                    src.Status == 2 ? "Shipping" :
                    src.Status == 3 ? "Completed" :
                    src.Status == 4 ? "Cancelled" : "Unknown"
                ));
        }
    }
}
