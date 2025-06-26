using BookManagement.BusinessObjects.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.IRepositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
    }
}
