using AutoMapper;
using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.DTOs.Book;
using BookManagement.Services.DTOs.Category;
using BookManagement.Services.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Auth DTOs
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ReverseMap();

            // User DTOs
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap(typeof(PagedResult<UserDto>), typeof(PagedResult<User>)).ReverseMap();


            // Category DTOs
            CreateMap<Category, CateDto>()
            .ForMember(dest => dest.ParentCategoryName,
                       opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
            .ReverseMap();
            CreateMap<Category, CreateCateDto>().ReverseMap();
            CreateMap<Category, UpdateCateDto>().ReverseMap();
            CreateMap(typeof(PagedResult<CateDto>), typeof(PagedResult<Category>)).ReverseMap();

        }
    }
}
