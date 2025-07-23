using BookManagement.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.IServices
{
    public interface ICartService
    {
        Task AddCartItemAsync(CartItem cartItem);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
        Task RemoveCartItemsAsync(string userId, List<int> cartItemIds);
        Task ClearCartAsync(string userId);
        public Task<int> GetCartCountAsync(string userId);
        public Task<CartItem?> GetCartItemByBookIdAsync(int bookId, string userSId);
        Task DeleteSoldOutItems(List<int> itemIds);
    }
}
