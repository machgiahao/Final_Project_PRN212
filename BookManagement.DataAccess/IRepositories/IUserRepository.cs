using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.IRepositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(string? id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<PagedResult<User>> GetAllUsersAsync(int pageNumber, int pageSize, string? name, string? email, string? phone);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
    }
}
