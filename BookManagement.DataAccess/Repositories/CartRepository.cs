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
    public class CartRepository : ICartRepository
    {
        private readonly BookStoreDbContext _context;

        public CartRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            try
            {
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding the cart item to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the cart item.", ex);
            }
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            try
            {
                return await _context.CartItems
                    .Include(ci => ci.Book)
                    .Include(ci => ci.User)
                    .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the cart item with ID {cartItemId}.", ex);
            }
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            try
            {
                return await _context.CartItems
                    .Include(ci => ci.Book)
                    .Where(ci => ci.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving cart items for user ID {userId}.", ex);
            }
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            try
            {
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred while updating the cart item.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the cart item in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the cart item.", ex);
            }
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartItems.FindAsync(cartItemId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Cart item with ID {cartItemId} not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the cart item from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the cart item.", ex);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            try
            {
                var items = _context.CartItems.Where(ci => ci.UserId == userId);
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while clearing the cart for user ID {userId}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred while clearing the cart for user ID {userId}.", ex);
            }
        }

        public Task<int> GetCartCountAsync(string userId)
        {
            try
            {
                return _context.CartItems.CountAsync(ci => ci.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the cart count for user ID {userId}.", ex);
            }
        }
    }
}
