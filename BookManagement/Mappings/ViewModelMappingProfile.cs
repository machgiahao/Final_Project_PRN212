using AutoMapper;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.DTOs.User;
using BookManagement.ViewModels.Auth;
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
        }
    }
}
