using AutoMapper;
using BCrypt.Net;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.DTOs.User;
using BookManagement.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDto?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userRepo.GetUserByEmailAsync(loginDto.Email);
                if (user == null) return null;

                if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                    return _mapper.Map<UserDto>(user);

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error during login for user '{loginDto.Email}': {ex.Message}", ex);
            }
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(id);
                return user == null ? null : _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving user by id '{id}': {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all users: {ex.Message}", ex);
            }
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var user = _mapper.Map<User>(registerDto);
                user.UserId = Guid.NewGuid().ToString();
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.Role = "Customer";
                await _userRepo.AddUserAsync(user);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error registering user '{registerDto.Email}': {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(string id, UserUpdateDto updateDto)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(id);
                if (user == null) return false;

                _mapper.Map(updateDto, user);
                await _userRepo.UpdateUserAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating user '{id}': {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(id);
                if (user == null) return false;
                await _userRepo.DeleteUserAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting user '{id}': {ex.Message}", ex);
            }
        }
    }
}
