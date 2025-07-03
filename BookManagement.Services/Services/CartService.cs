using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using BookManagement.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.Services
{
    public class CartService: ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task AddCartItemAsync(CartItem cartItem)
        {
            cartItem.CreatedAt = DateTime.UtcNow;
            await _cartRepository.AddCartItemAsync(cartItem);
        }
        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _cartRepository.GetCartItemByIdAsync(cartItemId);
        }
        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            return await _cartRepository.GetCartItemsByUserIdAsync(userId);
        }
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            await _cartRepository.UpdateCartItemAsync(cartItem);
        }
        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartRepository.DeleteCartItemAsync(cartItemId);
        }
        public async Task ClearCartAsync(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
        }
        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _cartRepository.GetCartCountAsync(userId);
        }
    }
}
