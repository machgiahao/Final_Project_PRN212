using Microsoft.EntityFrameworkCore;
using BookManagement.DataAccess.Context;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreDbContext _context;

        public UserRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding the user to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the user.", ex);
            }
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user with ID {id}.", ex);
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user with email {email}.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users.", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred while updating the user.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the user in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the user.", ex);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the user from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the user.", ex);
            }
        }
    }
}
