using AutoMapper;
using BookManagement.Services.DTOs.Auth;
using BookManagement.ViewModels;

namespace BookManagement.Mappings
{
    public class ViewModelMappingProfile: Profile
    {
        public ViewModelMappingProfile()
        {
             CreateMap<LoginViewModel, LoginDto>();
        }   
    }
}
