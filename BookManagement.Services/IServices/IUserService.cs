using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.IServices
{
    public interface IUserService
    {
        Task<UserDto?> LoginAsync(LoginDto loginDto);
        Task<UserDto?> GetByIdAsync(string id);
        Task<PagedResult<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? name, string? email, string? phone);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> CreateAsync(CreateUserDto userDto);
        Task<bool> UpdateAsync(string id, UserUpdateDto updateDto);
        Task<bool> DeleteAsync(string id);
        Task<UserDto?> LoginGoogleAsync(string email, string? name);
    }
}
