using AutoMapper;
using BookManagement.Services.DTOs.Auth;
using BookManagement.ViewModels.Auth;

namespace BookManagement.Mappings
{
    public class ViewModelMappingProfile: Profile
    {
        public ViewModelMappingProfile()
        {
             CreateMap<LoginViewModel, LoginDto>();
             CreateMap<RegisterViewModel, RegisterDto>();
        }   
    }
}
